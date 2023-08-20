using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Components;
using RogueStarIdle.ServerApplication.Shared.State;
using System.Reflection.Metadata.Ecma335;
using static RogueStarIdle.PlugIns.InMemory.MobsRepository;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class ActionState {
        public bool IsExploring { get; set; } = false;
        public bool IsInCombat { get; set; } = false;
        public bool IsCrafting { get; set; } = false;
        public bool IsScrapping { get; set; } = false;
        public bool MobsAreSpawned { get; set; } = false;
        public int TicksBetweenAction { get; set; } = 100;
        public int TicksUntilAction { get; set; } = 0;
        public List<ItemDrop>? ExploreableItems { get; set; } = null;
        public List<Item>? ExploredItems { get; set; } = null;
        public List<Item>? SelectedStorage { get; set; } = null;
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public List<MobSpawn> SpawnedMobs = new List<MobSpawn>();
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        public string location = "";
        public int SurvivalXpAtLocation { get; set; } = 0;
        public int respawnTime = 200; //4 sec
        public int respawnCounter = 200;
        public int healTime = 500;
        public CraftingRecipe SelectedCraftingRecipe { get; set; } = new CraftingRecipe();
        public ScrapRecipe SelectedScrapRecipe { get; set; } = new ScrapRecipe();
        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }

        public ActionState(InventoryState inventoryState, CharacterState characterState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
        }

        public void LeaveExploring()
        {
            IsExploring = false;
            IsInCombat = false;
        }

        public void LeaveCombat()
        {
            SpawnedMobs?.Clear();
            IsInCombat = false;
        }

        public async void ExploreTicks(int ticksElapsed)
        {
            if (!IsExploring || IsInCombat)
            {
                return;
            }

            // Bulk handling for time jumps
            if (ticksElapsed > TicksBetweenAction)
            {
                int exploreAttemptsToResolve = ticksElapsed / TicksBetweenAction;
                TicksUntilAction = ticksElapsed % TicksBetweenAction;
                Explore(exploreAttemptsToResolve);
            }

            if (TicksUntilAction > 0)
            {
                TicksUntilAction--;
                return;
            }
            Explore();
            await NotifyStateChanged();
        }

        public async void ActionTicks(int ticksElapsed)
        {
            CombatTicks(ticksElapsed);
            ExploreTicks(ticksElapsed);
            CraftTicks(ticksElapsed);
        }

        public async void CombatTicks(int ticksElapsed)
        {
            healTime = 500; // 10 sec to full health;
            if (IsExploring)
            {
                healTime = 6000; // 2 min to full health
            }
            //characterState?.MainCharacter.PassiveHeal(healTime);

            if (!IsInCombat)
            {
                return;
            }

            if (characterState.MainCharacter.IsAlive == false && !characterState.Characters.Any(c => c.IsAlive == true))
            {
                return;
            }

            for (int i = 0; i < ticksElapsed; i++)
            {

                if (MobsAreSpawned == false)
                {
                    respawnCounter -= 1;
                    if (IsExploring)
                    {
                        respawnCounter = 0;
                    }
                    if (respawnCounter > 0)
                    {
                        continue;
                    }
                    respawnCounter = respawnTime;
                    MobsAreSpawned = true;
                    SpawnMobs();
                }
                if (characterState.MainCharacter.IsAlive)
                {
                    characterState.MainCharacter.ActionCounter -= 1;
                    if (characterState.MainCharacter.ActionCounter <= 0)
                    {
                        characterState.MainCharacter.Equipment.CalculateStats(characterState.MainCharacter);
                        characterState.MainCharacter.TakeAction(characterState.MainCharacter, characterState.Characters, SpawnedMobs);
                        characterState.MainCharacter.ActionCounter = characterState.MainCharacter.Equipment.Stats.AttackSpeed;
                    }
                }
                foreach (MobSpawn mobSpawn in SpawnedMobs.ToList())
                {
                    mobSpawn.AttackCounter -= 1;
                    if (mobSpawn.AttackCounter <= 0)
                    {
                        mobSpawn.Mob.Attack(characterState.MainCharacter);
                        mobSpawn.AttackCounter = mobSpawn.Mob.Stats.AttackSpeed;
                    }
                    if (mobSpawn.Mob.CurrentHealth <= 0)
                    {
                        mobSpawn.Mob.Die();
                    }
                    if (characterState.MainCharacter.CurrentHealth <= 0)
                    {
                        characterState.MainCharacter.Die();
                    }
                }
                if (SpawnedMobs.Count == 0)
                {
                    MobsAreSpawned = false;
                }

                if (characterState.MainCharacter.IsAlive == false && (characterState.Characters?.All(c => c.IsAlive == false) ?? false))
                {
                    LeaveCombat();
                    LeaveExploring();
                    characterState.MainCharacter.Revive();
                    foreach (Character character in characterState.Characters)
                    {
                        character.Revive();
                    }
                }

                if (SpawnedMobs.Count(m => m.Mob.IsAlive) == 0 && MobsAreSpawned)
                {
                    foreach (MobSpawn mobSpawn in SpawnedMobs.ToList())
                    {
                        Loot(mobSpawn);
                    }
                    SpawnedMobs.Clear();
                    if (IsExploring)
                    {
                        LeaveCombat();
                    }
                }
            }
            await NotifyStateChanged();
        }

        private static readonly object locker = new object();
        public void Explore(int attempts = 1)
        {
            if (!IsExploring)
            {
                return;
            }
            Random rand = new Random();
            if (rand.Next(10) < 5)
            {
                EnterCombat(PossibleMobs, location, SelectedStorage);
            }
            if (ExploreableItems == null)
            {
                return;
            }
            List<Item> foundItems = new List<Item>();
            for (int i = 0; i < attempts; i++)
            {
                foreach (var item in ExploreableItems)
                {
                    int roll = rand.Next(item.DropChanceDenominator);
                    if (roll < item.DropChanceNumerator)
                    {
                        int qty = rand.Next(item.QuantityRangeMax - item.QuantityRangeMin + 1) + item.QuantityRangeMin;
                        item.Item.Quantity = qty;
                        foundItems.Add(item.Item);
                    }
                }
            }
            lock (locker)
            {
                foreach (var item in foundItems)
                {
                    inventoryState.AddToInventory(SelectedStorage, item, item.Quantity);
                }
            }
            characterState.MainCharacter.SurvivalSkill.Xp += SurvivalXpAtLocation * attempts;
            characterState.MainCharacter.SurvivalSkill.UpdateLevel();
            TicksUntilAction = TicksBetweenAction;
            return;
        }
        public async void CraftTicks(int ticksElapsed)
        {
            if (!IsCrafting && !IsScrapping)
            {
                return;
            }

            // Bulk handling for time jumps
            if (ticksElapsed > TicksBetweenAction)
            {
                int actionsToResolve = ticksElapsed / TicksBetweenAction;
                TicksUntilAction = ticksElapsed % TicksBetweenAction;
                for (int i = 0; i < actionsToResolve; i++)
                {
                    if (IsCrafting)
                    {
                        Craft();
                    }
                    if (IsScrapping)
                    {
                        Scrap();
                    }
                }
            }

            if (TicksUntilAction > 0)
            {
                TicksUntilAction--;
                await NotifyStateChanged();
                return;
            }
            TicksUntilAction = TicksBetweenAction;
            if (IsCrafting)
            {
                Craft();
            }
            if (IsScrapping)
            {
                Scrap();
            }
            await NotifyStateChanged();
        }

        public void EnterExplore(List<ItemDrop> scavengables, List<MobSpawn> mobs, string location, List<Item> locationStorage)
        {
            this.location = location;
            IsExploring = true;
            IsCrafting = false;
            LeaveCombat();
            ExploreableItems = scavengables;
            PossibleMobs = new List<MobSpawn>(mobs);
            SurvivalXpAtLocation = 1;
            SelectedStorage = locationStorage;
        }

        public void SpawnMobs()
        {
            Random rand = new Random();
            // TODO randomize selection
            SpawnedMobs.Add(PossibleMobs[(int)MobsEnum.Carapig].Clone());
            SpawnedMobs.Add(PossibleMobs[(int)MobsEnum.Tardihop].Clone());
            SpawnedMobs.Add(PossibleMobs[(int)MobsEnum.Tardihop].Clone());
            SpawnedMobs.Add(PossibleMobs[(int)MobsEnum.Tardihop].Clone());
            SpawnedMobs.Add(PossibleMobs[(int)MobsEnum.Tardihop].Clone());
            SpawnedMobs[3].Mob.CurrentHealth = 20;
            SpawnedMobs[4].Mob.CurrentHealth = 400; 
            SpawnedMobs[3].Mob.Stats.MaxHealth = 20;
            SpawnedMobs[4].Mob.Stats.MaxHealth = 400;
            foreach (MobSpawn mobSpawn in SpawnedMobs)
            {
                mobSpawn.AttackCounter = rand.Next(mobSpawn.Mob.Stats.AttackSpeed + 1);
                mobSpawn.Mob.CurrentHealth = mobSpawn.Mob.Stats.MaxHealth;
                mobSpawn.Mob.IsAlive = true;
            }
            MobsAreSpawned = true;
        }

        public void Loot(MobSpawn mobSpawn)
        {

            Random rand = new Random();
            foreach (ItemDrop itemDrop in mobSpawn.Loot)
            {
                int roll = rand.Next(itemDrop.DropChanceDenominator);
                if (roll < itemDrop.DropChanceNumerator)
                {
                    int qty = itemDrop.QuantityRangeMin + rand.Next(itemDrop.QuantityRangeMax - itemDrop.QuantityRangeMin + 1);
                    itemDrop.Item.Quantity = qty;
                    inventoryState.AddToInventory(SelectedStorage, itemDrop.Item, itemDrop.Item.Quantity);
                }
            }
        }

        public void EnterCombat(List<MobSpawn> mobs, string location, List<Item> locationStorage)
        {
            characterState.MainCharacter.Equipment.CalculateStats(characterState.MainCharacter);
            this.location = location;
            IsInCombat = true;
            IsCrafting = false;
            IsScrapping = false;
            PossibleMobs = new List<MobSpawn>(mobs);
            SelectedStorage = locationStorage;
        }

        public void EnterCrafting()
        {
            TicksUntilAction = TicksBetweenAction;
            IsCrafting = true;
            IsScrapping = false;
            IsInCombat = false;
            IsExploring = false;
        }

        public void EnterScrapping()
        {
            TicksUntilAction = TicksBetweenAction;
            IsCrafting = false;
            IsScrapping = true;
            IsInCombat = false;
            IsExploring = false;
        }

        public void LeaveCrafting()
        {
            IsCrafting = false;
        }

        public void LeaveScrapping()
        {
            IsScrapping = false;
        }

        public void Craft()
        {
            bool sufficientMaterials = true;
            if (IsCrafting)
            {
                foreach ((Item ingredient, int qty) in SelectedCraftingRecipe.Ingredients)
                {
                    Item item = inventoryState.Inventory.FirstOrDefault(i => i.Name == ingredient.Name);
                    if (item == null || item.Quantity < qty)
                    {
                        sufficientMaterials = false;
                    }
                }
                if (!sufficientMaterials)
                {
                    return;
                }
                foreach ((Item ingredient, int qty) in SelectedCraftingRecipe.Ingredients)
                {
                    inventoryState.RemoveFromInventory(inventoryState.Inventory, ingredient, qty);
                }
                inventoryState.AddToInventory(inventoryState.Inventory, SelectedCraftingRecipe.Item);
                switch (SelectedCraftingRecipe.XpReward.Item1)
                {
                    case ("Survival"): characterState.MainCharacter.SurvivalSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                    default: break;
                }
                switch (SelectedCraftingRecipe.XpReward.Item1)
                {
                    case "Survival":
                        characterState.MainCharacter.SurvivalSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                    case "Armor Crafting":
                        characterState.MainCharacter.ArmorCraftingSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                    case "Weapon Crafting":
                        characterState.MainCharacter.WeaponCraftingSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                    case "Construction":
                        characterState.MainCharacter.ConstructionSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                    case "Medicine":
                        characterState.MainCharacter.MedicineSkill.addXp(SelectedCraftingRecipe.XpReward.Item2);
                        break;
                }
            }
        }

        public void Scrap()
        {
            bool sufficientMaterials = true;
            if (IsScrapping)
            {
                Item item = inventoryState.Inventory.FirstOrDefault(i => i.Name == SelectedScrapRecipe.Item.Name);
                if (item == null || item.Quantity == 0)
                {
                    sufficientMaterials = false;
                }
                if (!sufficientMaterials)
                {
                    return;
                }
                inventoryState.RemoveFromInventory(inventoryState.Inventory, item);
                Random rand = new Random();
                characterState.MainCharacter.ScrappingSkill.addXp(1);
                foreach (Scrap scrap in SelectedScrapRecipe.ScrapList)
                {
                    if (rand.Next(scrap.Denominator) < scrap.Numerator)
                    {
                        int amount = scrap.QuantityMin + rand.Next(scrap.QuantityMax - scrap.QuantityMin);
                        inventoryState.AddToInventory(inventoryState.Inventory, scrap.Item, amount);
                    }
                }
            }

        }
    }
}
