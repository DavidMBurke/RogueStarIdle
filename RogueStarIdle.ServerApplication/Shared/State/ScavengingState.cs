using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class ScavengingState {
        public bool IsScavenging { get; set; } = false;
        public int TicksBetweenScavengeAttempts { get; set; } = 100;
        public int TicksUntilScavengeAttempt { get; set; } = 100;
        public List<ItemDrop>? ScavengeableItems { get; set; } = null;
        public List<Item>? ScavengedItems { get; set; } = null;
        public List<Item>? SelectedStorage { get; set; } = null;
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        public int SurvivalXpAtLocation { get; set; } = 0;
        public ScavengingState(InventoryState inventoryState, CharacterState characterState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
        }
        public void ScavengeTicks (int ticksElapsed)
        {   
            if (!IsScavenging)
            {
                return;
            }

            // Bulk handling for time jumps
            if (ticksElapsed > TicksBetweenScavengeAttempts)
            {
                int scavengeAttemptsToResolve = ticksElapsed / TicksBetweenScavengeAttempts;
                TicksUntilScavengeAttempt = ticksElapsed % TicksBetweenScavengeAttempts;
                Scavenge(scavengeAttemptsToResolve);
            }

            if (TicksUntilScavengeAttempt > 0)
            {
                TicksUntilScavengeAttempt--;
                return;
            }
            Scavenge();
            
        }

        private static readonly object locker = new object();
        public void Scavenge(int attempts = 1)
        {
            if (ScavengeableItems == null)
            {
                return;
            }
            List<Item> foundItems = new List<Item>();
            Random rand = new Random();
            for (int i = 0; i < attempts; i++)
            {
                foreach (var item in ScavengeableItems)
                {
                    int roll = rand.Next(item.DropChanceDenominator) + 1;
                    if (roll <= item.DropChanceNumerator)
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
            characterState.mainCharacter.SurvivalSkill.Xp += SurvivalXpAtLocation * attempts;
            characterState.mainCharacter.SurvivalSkill.UpdateLevel();
            TicksUntilScavengeAttempt = TicksBetweenScavengeAttempts;
            return;
        }
    }
}
