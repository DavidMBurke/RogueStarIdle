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
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public List<MobSpawn> SpawnedMobs = new List<MobSpawn>();
        public int respawnTime = 200; //4 sec
        public int respawnCounter = 200;
        public CombatState(InventoryState inventoryState, CharacterState characterState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
        }
        public void CombatTicks(int ticksElapsed)
        {
            if (!IsInCombat) { 
                return; 
            }

            for (int i = 0; i < ticksElapsed; i++)
            {

                if (MobsAreSpawned == false)
                {
                    respawnCounter -= 1;
                    if (respawnCounter <= 0)
                    {
                        respawnCounter = respawnTime;
                        MobsAreSpawned = true;
                        SpawnMobs();
                    } else
                    {
                        return;
                    }
                }
            
                characterState.mainCharacter.AttackCounter -= 1;
                if (characterState.mainCharacter.AttackCounter <= 0)
                {
                    characterState.mainCharacter.Equipment.CalculateStats();
                    characterState.mainCharacter.Attack(SpawnedMobs.FirstOrDefault());
                    characterState.mainCharacter.AttackCounter = characterState.mainCharacter.Equipment.Stats.AttackSpeed;
                }
                foreach (MobSpawn mobSpawn in SpawnedMobs)
                {
                    mobSpawn.AttackCounter -= 1;
                    if (mobSpawn.AttackCounter <= 0)
                    {
                        mobSpawn.Mob.Attack(characterState.mainCharacter);
                        mobSpawn.AttackCounter = mobSpawn.Mob.Stats.AttackSpeed;
                    }
                    if (mobSpawn.Mob.Stats.CurrentHealth <= 0)
                    {
                        Console.WriteLine($"{mobSpawn.Mob.Name} has been slain!");
                        SpawnedMobs.Remove(mobSpawn);
                    }
                }
                if (SpawnedMobs.Count == 0)
                {
                    MobsAreSpawned = false;
                }
            }
        }

        public void SpawnMobs()
        {
            // TODO randomize selection
            SpawnedMobs.Add(PossibleMobs[0]);
            foreach (MobSpawn mob in SpawnedMobs)
            {
                mob.Mob.Stats.CurrentHealth = mob.Mob.Stats.MaxHealth;
            }
            MobsAreSpawned = true;
            Console.WriteLine($"{PossibleMobs[0].Mob.Name} spawned!");
        }
    }
}
