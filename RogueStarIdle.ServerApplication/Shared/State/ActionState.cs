using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Components;
using RogueStarIdle.ServerApplication.Shared.State;
using System.Reflection.Metadata.Ecma335;
using static RogueStarIdle.CoreBusiness.CombatRule;
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
            characterState?.MainCharacter.PassiveHeal(healTime);
            characterState?.Characters.ForEach((c) => c.PassiveHeal(healTime));

            if (!IsInCombat)
            {
                return;
            }

            if (characterState.MainCharacter.IsAlive == false && characterState.Characters.All(c => c.IsAlive == false))
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
                        TakeAction(characterState.MainCharacter, characterState.Characters, SpawnedMobs);
                        characterState.MainCharacter.ActionCounter = characterState.MainCharacter.Equipment.Stats.AttackSpeed;
                    }
                }
                foreach (Character character in characterState.Characters)
                {
                    if (character.IsAlive)
                    {
                        character.ActionCounter -= 1;
                        if (character.ActionCounter <= 0)
                        {
                            character.Equipment.CalculateStats(character);
                            TakeAction(character, characterState.Characters, SpawnedMobs);
                            character.ActionCounter = character.Equipment.Stats.AttackSpeed;
                        }
                    }
                }

                foreach (MobSpawn mobSpawn in SpawnedMobs.ToList())
                {
                    mobSpawn.AttackCounter -= 1;
                    if (mobSpawn.AttackCounter <= 0)
                    {
                        if (characterState.MainCharacter.IsAlive)
                        {
                            mobSpawn.Mob.Attack(characterState.MainCharacter);
                        } else if (characterState.Characters.Any(c => c.IsAlive)) {
                            mobSpawn.Mob.Attack(characterState.Characters.First(c => c.IsAlive));
                        }
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
                    foreach (Character character in characterState.Characters)
                    {
                        if (character.CurrentHealth <= 0)
                        {
                            character.Die();
                        }
                    }
                }
                if (SpawnedMobs.Count == 0)
                {
                    MobsAreSpawned = false;
                }

                if (characterState.MainCharacter.IsAlive == false && characterState.Characters.All(c => c.IsAlive == false))
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
            characterState.Characters.ForEach(c => c.Equipment.CalculateStats(c));
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
        public void TakeAction(Character actionCharacter, List<Character> allies, List<MobSpawn> enemies)
        {
            bool actionTaken = false;
            foreach (CombatRule rule in actionCharacter.CombatRules)
            {
                switch (rule.Action)
                {
                    case (int)ActionEnum.Attack: actionTaken = Attack(actionCharacter, enemies, rule); break;
                    case (int)ActionEnum.Heal: actionTaken = Heal(actionCharacter, allies, rule); break;
                }
                if (actionTaken)
                { break; }
            }
        }

        public bool Attack(Character actionCharacter, List<MobSpawn> defenders, CombatRule rule)
        {
            if (defenders == null && rule.Target == (int)TargetEnum.Enemy)
            {
                return false;
            }
            List<MobSpawn> aliveDefenders = defenders.Where(d => d.Mob.IsAlive).ToList();
            if (aliveDefenders.Count == 0 && rule.Target == (int)TargetEnum.Enemy)
            {
                return false;
            }
            actionCharacter.TriggerAttackAnimation = true;

            Random rand = new Random();
            MobSpawn defender = new MobSpawn();

            switch (rule.TargetQualifier)
            {
                case (int)TargetQualifierEnum.Any:
                    {
                        defender = aliveDefenders.First();
                        break;
                    }
                case (int)TargetQualifierEnum.Lowest:
                    {
                        defender = getDefenderByStat(aliveDefenders, rule.TargetQualifierStat, rule.TargetQualifier);
                        break;
                    }
                case (int)TargetQualifierEnum.Highest:
                    {
                        defender = getDefenderByStat(aliveDefenders, rule.TargetQualifierStat, rule.TargetQualifier);
                        break;
                    }
            }

            // TODO: Add Conditionals
            Stats s = actionCharacter.Equipment.Stats;
            int hitRoll = rand.Next(20);
            if (s.IsUsingMelee)
            {
                hitRoll += s.MeleeToHit;
            }
            if (s.IsUsingRanged)
            {
                hitRoll += s.RangedToHit;
            }
            if (s.IsUsingExplosive)
            {
                hitRoll += s.ExplosiveToHit;
            }
            if (s.IsUsingPsychic)
            {
                hitRoll += s.PsychicToHit;
            }

            int xp = 1;
            int blockRoll = defender.Mob.Stats.MeleeDefense + rand.Next(20);
            if (hitRoll > blockRoll)
            {
                int damage = CalculateTotalDamage(s, defender.Mob.Stats);
                defender.Mob.CurrentHealth -= damage;
                if (defender.Mob.CurrentHealth < 0) { 
                    defender.Mob.CurrentHealth = 0; 
                }
                xp = 2;
            }
            if (s.IsUsingMelee)
            {
                actionCharacter.MeleeSkill.addXp(xp);
            }
            if (s.IsUsingRanged)
            {
                actionCharacter.RangedSkill.addXp(xp);
            }
            if (s.IsUsingExplosive)
            {
                actionCharacter.ExplosivesSkill.addXp(xp);
            }
            if (s.IsUsingPsychic)
            {
                actionCharacter.PsychicSkill.addXp(xp);
            }
            return true;
        }

        public bool Heal(Character actionCharacter, List<Character> allies, CombatRule rule)
        {
            Character characterToHeal = characterState.MainCharacter;
            if (rule.Target == (int)TargetEnum.Ally)
            {
                if (rule.TargetQualifier == (int)TargetQualifierEnum.Any)
                {
                    characterToHeal = allies.FirstOrDefault(c => c.CurrentHealth < c.Equipment.Stats.MaxHealth);
                    if (characterToHeal == null) { return false; }
                }
                characterToHeal = getAllyByStat(allies, rule.TargetQualifierStat, rule.TargetQualifier);
            }
            if (actionCharacter.Equipment.Aid.Item == null)
            { return false; }
            if (!actionCharacter.Equipment.Aid.Item.Consumable ||
                actionCharacter.Equipment.Aid.Item.HealthRestored <= 0 ||
                actionCharacter.Equipment.Aid.Item.Quantity <= 0)
            {
                return false;
            }
            characterToHeal.CurrentHealth += actionCharacter.Equipment.Aid.Item.HealthRestored;
            if (characterToHeal.CurrentHealth > characterToHeal.Equipment.Stats.MaxHealth)
            {
                characterToHeal.CurrentHealth = characterToHeal.Equipment.Stats.MaxHealth;
            }
            inventoryState.ConsumeEquipped(inventoryState.Inventory, actionCharacter.Equipment.Aid.Item, 1);
            return true;
        }

        private MobSpawn getDefenderByStat(List<MobSpawn> defenders, int targetQualifierStat, int targetQualifier)
        {
            if (targetQualifier == (int)TargetQualifierEnum.Lowest)
            {
                switch (targetQualifierStat)
                {
                    case (int)StatDropdownEnum.CurrentHealth:
                        return defenders.OrderBy(d => d.Mob.CurrentHealth).First();
                    case (int)StatDropdownEnum.MaxHealth:
                        return defenders.OrderBy(d => d.Mob.Stats.MaxHealth).First();
                    case (int)StatDropdownEnum.MeleeToHit:
                        return defenders.OrderBy(d => d.Mob.Stats.MeleeToHit).First();
                    case (int)StatDropdownEnum.MeleeDamageMax:
                        return defenders.OrderBy(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.RangedToHit:
                        return defenders.OrderBy(d => d.Mob.Stats.RangedToHit).First();
                    case (int)StatDropdownEnum.RangedDamageMax:
                        return defenders.OrderBy(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.PsychicToHit:
                        return defenders.OrderBy(d => d.Mob.Stats.PsychicToHit).First();
                    case (int)StatDropdownEnum.ExplosiveToHit:
                        return defenders.OrderBy(d => d.Mob.Stats.ExplosiveToHit).First();
                    default:
                        return defenders.OrderBy(d => d.Mob.CurrentHealth).First();
                }
            }
            switch (targetQualifierStat)
            {
                case (int)StatDropdownEnum.CurrentHealth:
                    return defenders.OrderByDescending(d => d.Mob.CurrentHealth).First();
                case (int)StatDropdownEnum.MaxHealth:
                    return defenders.OrderByDescending(d => d.Mob.Stats.MaxHealth).First();
                case (int)StatDropdownEnum.MeleeToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.MeleeToHit).First();
                case (int)StatDropdownEnum.MeleeDamageMax:
                    return defenders.OrderByDescending(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.RangedToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.RangedToHit).First();
                case (int)StatDropdownEnum.RangedDamageMax:
                    return defenders.OrderByDescending(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.PsychicToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.PsychicToHit).First();
                case (int)StatDropdownEnum.ExplosiveToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.ExplosiveToHit).First();
                default:
                    return defenders.OrderByDescending(d => d.Mob.CurrentHealth).First();
            }
        }

        private Character getAllyByStat(List<Character> allies, int targetQualifierStat, int targetQualifier)
        {
            if (targetQualifier == (int)TargetQualifierEnum.Lowest)
            {
                switch (targetQualifierStat)
                {
                    case (int)StatDropdownEnum.CurrentHealth:
                        return allies.OrderBy(d => d.CurrentHealth).First();
                    case (int)StatDropdownEnum.MaxHealth:
                        return allies.OrderBy(d => d.Equipment.Stats.MaxHealth).First();
                    case (int)StatDropdownEnum.MeleeToHit:
                        return allies.OrderBy(d => d.Equipment.Stats.MeleeToHit).First();
                    case (int)StatDropdownEnum.MeleeDamageMax:
                        return allies.OrderBy(d => new[] { d.Equipment.Stats.CrushingDamageMax, d.Equipment.Stats.SlashingDamageMax, d.Equipment.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.RangedToHit:
                        return allies.OrderBy(d => d.Equipment.Stats.RangedToHit).First();
                    case (int)StatDropdownEnum.RangedDamageMax:
                        return allies.OrderBy(d => new[] { d.Equipment.Stats.CrushingDamageMax, d.Equipment.Stats.SlashingDamageMax, d.Equipment.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.PsychicToHit:
                        return allies.OrderBy(d => d.Equipment.Stats.PsychicToHit).First();
                    case (int)StatDropdownEnum.ExplosiveToHit:
                        return allies.OrderBy(d => d.Equipment.Stats.ExplosiveToHit).First();
                    default:
                        return allies.OrderBy(d => d.CurrentHealth).First();
                }
            }
            switch (targetQualifierStat)
            {
                case (int)StatDropdownEnum.CurrentHealth:
                    return allies.OrderByDescending(d => d.CurrentHealth).First();
                case (int)StatDropdownEnum.MaxHealth:
                    return allies.OrderByDescending(d => d.Equipment.Stats.MaxHealth).First();
                case (int)StatDropdownEnum.MeleeToHit:
                    return allies.OrderByDescending(d => d.Equipment.Stats.MeleeToHit).First();
                case (int)StatDropdownEnum.MeleeDamageMax:
                    return allies.OrderByDescending(d => new[] { d.Equipment.Stats.CrushingDamageMax, d.Equipment.Stats.SlashingDamageMax, d.Equipment.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.RangedToHit:
                    return allies.OrderByDescending(d => d.Equipment.Stats.RangedToHit).First();
                case (int)StatDropdownEnum.RangedDamageMax:
                    return allies.OrderByDescending(d => new[] { d.Equipment.Stats.CrushingDamageMax, d.Equipment.Stats.SlashingDamageMax, d.Equipment.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.PsychicToHit:
                    return allies.OrderByDescending(d => d.Equipment.Stats.PsychicToHit).First();
                case (int)StatDropdownEnum.ExplosiveToHit:
                    return allies.OrderByDescending(d => d.Equipment.Stats.ExplosiveToHit).First();
                default:
                    return allies.OrderByDescending(d => d.CurrentHealth).First();
            }
        }


        public int CalculateTotalDamage(Stats attacker, Stats defender)
        {
            Random rand = new Random();
            int damage = 0;
            damage += CalculateDamageByType(attacker.PiercingDamageMin, attacker.PiercingDamageMax, defender.PiercingDR);
            damage += CalculateDamageByType(attacker.SlashingDamageMin, attacker.SlashingDamageMax, defender.SlashingDR);
            damage += CalculateDamageByType(attacker.CrushingDamageMin, attacker.CrushingDamageMax, defender.CrushingDR);
            damage += CalculateDamageByType(attacker.AcidDamageMin, attacker.AcidDamageMax, defender.AcidDR);
            damage += CalculateDamageByType(attacker.FireDamageMin, attacker.FireDamageMax, defender.FireDR);
            damage += CalculateDamageByType(attacker.ShockDamageMin, attacker.ShockDamageMax, defender.ShockDR);
            damage += CalculateDamageByType(attacker.PoisonDamageMin, attacker.PoisonDamageMax, defender.PoisonDR);
            return damage;
        }

        public int CalculateDamageByType(int min, int max, int dr)
        {
            Random rand = new Random();
            int baseDamage = min + rand.Next(1 + max - min);
            int reducedDamage = (baseDamage * (100 - dr)) / 100;
            return reducedDamage;
        }

    }
}
