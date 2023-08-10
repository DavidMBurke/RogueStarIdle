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
        public string combatLocation = "";
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public List<MobSpawn> SpawnedMobs = new List<MobSpawn>();
        public int respawnTime = 200; //4 sec
        public int respawnCounter = 200;
        public event Func<Task> OnChange;
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
            PossibleMobs.Clear();
            SpawnedMobs.Clear();
            combatLocation = "";
            IsInCombat = false;
        }
        public async void CombatTicks(int ticksElapsed)
        {
            if (!IsInCombat) { 
                return; 
            }

            for (int i = 0; i < ticksElapsed; i++)
            {

                if (MobsAreSpawned == false)
                {
                    respawnCounter -= 1;
                    if (respawnCounter > 0)
                    {
                        continue;
                    }
                    respawnCounter = respawnTime;
                    MobsAreSpawned = true;
                    SpawnMobs();
                }
                characterState.MainCharacter.AttackCounter -= 1;
                if (characterState.MainCharacter.AttackCounter <= 0)
                {
                    characterState.MainCharacter.Equipment.CalculateStats(characterState.MainCharacter);
                    characterState.MainCharacter.Attack(SpawnedMobs.FirstOrDefault());
                    characterState.MainCharacter.AttackCounter = characterState.MainCharacter.Equipment.Stats.AttackSpeed;
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
                        Console.WriteLine($"{mobSpawn.Mob.Name} has been slain!");
                        Loot(mobSpawn);
                        SpawnedMobs.Remove(mobSpawn);
                    }
                }
                if (SpawnedMobs.Count == 0)
                {
                    MobsAreSpawned = false;
                }
            }
            await NotifyStateChanged();
        }

        public void SpawnMobs()
        {
            // TODO randomize selection
            SpawnedMobs.Add(PossibleMobs[0]);
            foreach (MobSpawn mob in SpawnedMobs)
            {
                mob.Mob.CurrentHealth = mob.Mob.Stats.MaxHealth;
            }
            MobsAreSpawned = true;
            Console.WriteLine($"{PossibleMobs[0].Mob.Name} spawned!");
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
                    Console.WriteLine($"{mobSpawn.Mob.Name} dropped {qty} X {itemDrop.Item.Name}");
                }
            }
        }
    }
}
