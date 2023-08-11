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
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        public CombatState? combatState;
        public string ScavengeLocation = "";
        public int SurvivalXpAtLocation { get; set; } = 0;
        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }
        public ScavengingState(InventoryState inventoryState, CharacterState characterState, CombatState combatState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
            this.combatState = combatState;
        }

        public void LeaveScavenging()
        {
            IsScavenging = false;
        }
        public async void ScavengeTicks (int ticksElapsed)
        {   
            if (!IsScavenging || combatState.IsInCombat)
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
            await NotifyStateChanged();
        }

        private static readonly object locker = new object();
        public void Scavenge(int attempts = 1)
        {
            Random rand = new Random();
            if (rand.Next(10) < 5)
            {
                combatState.EnterCombat(PossibleMobs, ScavengeLocation, SelectedStorage, isScavenging: true, LeaveScavenging);
            }
            if (ScavengeableItems == null)
            {
                return;
            }
            List<Item> foundItems = new List<Item>();
            for (int i = 0; i < attempts; i++)
            {
                foreach (var item in ScavengeableItems)
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
            TicksUntilScavengeAttempt = TicksBetweenScavengeAttempts;
            return;
        }

        public void SelectScavenge(List<ItemDrop> scavengables, List<MobSpawn> mobs, string location, List<Item> locationStorage)
        {
            if (IsScavenging)
            {
                LeaveScavenging();
                return;
            }
            ScavengeLocation = location;
            IsScavenging = true;
            combatState.LeaveCombat();
            ScavengeableItems = scavengables;
            PossibleMobs = new List<MobSpawn>(mobs);
            SurvivalXpAtLocation = 1;
            SelectedStorage = locationStorage;
        }

    }
}
