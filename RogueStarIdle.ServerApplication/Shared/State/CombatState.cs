using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CombatState {
        public bool IsInCombat { get; set; } = false;
        public bool MobsAreSpawned { get; set; } = false;
        public List<Item>? SelectedStorage { get; set; } = null;
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        //cannot use scavenging state d/t circular dependency, so this IsScavenging needs to sync with scavengingState instead
        public bool IsScavenging = false; 
        public string combatLocation = "";
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public List<MobSpawn> SpawnedMobs = new List<MobSpawn>();
        public int respawnTime = 200; //4 sec
        public int respawnCounter = 200;
        public event Func<Task> OnChange;
        public event Action LeaveScavenging;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }
        public CombatState(InventoryState inventoryState, CharacterState characterState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
        }

        public void LeaveCombat()
        {
            SpawnedMobs?.Clear();
            IsInCombat = false;
        }
        public async void CombatTicks(int ticksElapsed)
        {
            if (!IsInCombat) {
                characterState?.MainCharacter.PassiveHeal(500); //10 sec to full health
                return;
            }
            if (characterState.MainCharacter.IsAlive == false && !characterState.Characters.Any(c => c.IsAlive == true))
            {
                return;
            }
            characterState.MainCharacter.PassiveHeal(6000); //2 min to full health

            for (int i = 0; i < ticksElapsed; i++)
            {

                if (MobsAreSpawned == false)
                {
                    respawnCounter -= 1;
                    if (IsScavenging)
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
                    LeaveScavenging.Invoke();
                    characterState.MainCharacter.Revive();
                    foreach (Character character in characterState.Characters)
                    {
                        character.Revive();
                    }
                }

                if (SpawnedMobs.Count(m => m.Mob.IsAlive) == 0 && MobsAreSpawned)
                {
                    foreach (MobSpawn mobSpawn in SpawnedMobs) {
                        Loot(mobSpawn);
                    }
                    SpawnedMobs.Clear();
                    if (IsScavenging)
                    {
                        LeaveCombat();
                    }
                }
            }
            await NotifyStateChanged();
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
                if (roll < itemDrop.DropChanceNumerator) {
                    int qty = itemDrop.QuantityRangeMin + rand.Next(itemDrop.QuantityRangeMax - itemDrop.QuantityRangeMin + 1);
                    itemDrop.Item.Quantity = qty;
                    inventoryState.AddToInventory(SelectedStorage, itemDrop.Item, itemDrop.Item.Quantity);
                }
            }
        }

        public void EnterCombat(List<MobSpawn> mobs, string location, List<Item> locationStorage, bool isScavenging = false, Action? leaveScavenging = null)
        {
            characterState.MainCharacter.Equipment.CalculateStats(characterState.MainCharacter);
            combatLocation = location;
            IsInCombat = true;
            IsScavenging = isScavenging;
            PossibleMobs = new List<MobSpawn>(mobs);
            SelectedStorage = locationStorage;
            if (leaveScavenging != null)
            {
                LeaveScavenging += leaveScavenging;
            }
        }
    }
}
