using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class ActionState {
        public bool IsExploring { get; set; } = false;
        public bool IsInCombat { get; set; } = false;
        public bool MobsAreSpawned { get; set; } = false;

        public int TicksBetweenExploreAttempts { get; set; } = 100;
        public int TicksUntilExploreAttempt { get; set; } = 100;
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
        }
        public void LeaveCombat()
        {
            SpawnedMobs?.Clear();
            IsInCombat = false;
        }
        public async void ExploreTicks (int ticksElapsed)
        {   
            if (!IsExploring || IsInCombat)
            {
                return;
            }

            // Bulk handling for time jumps
            if (ticksElapsed > TicksBetweenExploreAttempts)
            {
                int exploreAttemptsToResolve = ticksElapsed / TicksBetweenExploreAttempts;
                TicksUntilExploreAttempt = ticksElapsed % TicksBetweenExploreAttempts;
                Explore(exploreAttemptsToResolve);
            }

            if (TicksUntilExploreAttempt > 0)
            {
                TicksUntilExploreAttempt--;
                return;
            }
            Explore();
            await NotifyStateChanged();
        }

        public async void ActionTicks(int ticksElapsed)
        {
            CombatTicks(ticksElapsed);
            ExploreTicks(ticksElapsed);
        }
        public async void CombatTicks(int ticksElapsed)
        {
            healTime = 500; // 10 sec to full health;
            if (IsExploring)
            {
                healTime = 6000; // 2 min to full health
            }
            characterState?.MainCharacter.PassiveHeal(healTime);

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
                    characterState.MainCharacter.AttackCounter -= 1;
                    if (characterState.MainCharacter.AttackCounter <= 0)
                    {
                        characterState.MainCharacter.Equipment.CalculateStats(characterState.MainCharacter);
                        characterState.MainCharacter.Attack(SpawnedMobs.FirstOrDefault(s => s.Mob.IsAlive));
                        characterState.MainCharacter.AttackCounter = characterState.MainCharacter.Equipment.Stats.AttackSpeed;
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
                        int qty = rand.Next(item.QuantityRangeMax-item.QuantityRangeMin + 1) + item.QuantityRangeMin;
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
            TicksUntilExploreAttempt = TicksBetweenExploreAttempts;
            return;
        }

        public void EnterExplore(List<ItemDrop> scavengables, List<MobSpawn> mobs, string location, List<Item> locationStorage)
        {
            this.location = location;
            IsExploring = true;
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
            SpawnedMobs.Add(PossibleMobs[0].Clone());
            SpawnedMobs.Add(PossibleMobs[0].Clone());
            SpawnedMobs.Add(PossibleMobs[0].Clone());
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
            PossibleMobs = new List<MobSpawn>(mobs);
            SelectedStorage = locationStorage;
        }
    }
}
