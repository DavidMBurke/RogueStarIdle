using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CombatState {
        public bool IsInCombat { get; set; } = false;
        public bool MobsSpawned { get; set; } = false;
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

                if (MobsSpawned == false)
                {
                    respawnCounter -= 1;
                    if (respawnCounter <= 0)
                    {
                        respawnCounter = respawnTime;
                        MobsSpawned = true;
                        SpawnMobs();
                    } else
                    {
                        return;
                    }
                }
            
                characterState.mainCharacter.AttackCounter -= 1;
                if (characterState.mainCharacter.AttackCounter <= 0)
                {
                    PlayerAttack();
                    characterState.mainCharacter.AttackCounter = characterState.mainCharacter.Equipment.StatBlock.AttackSpeed;
                }
                foreach (MobSpawn mob in SpawnedMobs)
                {
                    mob.AttackCounter -= 1;
                    if (mob.AttackCounter <= 0)
                    {
                        MobAttack();
                        mob.AttackCounter = mob.Mob.Stats.AttackSpeed;
                    }
                }
            }
        }

        public void PlayerAttack()
        {
            Console.WriteLine("Player attacks");
        }

        public void MobAttack()
        {
            Console.WriteLine("Mob attacks");
        }

        public void SpawnMobs()
        {
            SpawnedMobs.Add(PossibleMobs[0]);
            MobsSpawned = true;
            Console.WriteLine("Mob spawned!");
        }
    }
}
