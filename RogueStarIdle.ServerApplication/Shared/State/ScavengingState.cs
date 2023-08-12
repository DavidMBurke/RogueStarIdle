using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class ExploringState {
        public bool IsExploring { get; set; } = false;
        public int TicksBetweenExploreAttempts { get; set; } = 100;
        public int TicksUntilExploreAttempt { get; set; } = 100;
        public List<ItemDrop>? ExploreableItems { get; set; } = null;
        public List<Item>? ExploredItems { get; set; } = null;
        public List<Item>? SelectedStorage { get; set; } = null;
        public List<MobSpawn> PossibleMobs = new List<MobSpawn>();
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        public CombatState? combatState;
        public string exploreLocation = "";
        public int SurvivalXpAtLocation { get; set; } = 0;
        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }
        public ExploringState(InventoryState inventoryState, CharacterState characterState, CombatState combatState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
            this.combatState = combatState;
        }

        public void LeaveExploring()
        {
            IsExploring = false;
        }
        public async void ExploreTicks (int ticksElapsed)
        {   
            if (!IsExploring || combatState.IsInCombat)
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

        private static readonly object locker = new object();
        public void Explore(int attempts = 1)
        {
            Random rand = new Random();
            if (rand.Next(10) < 5)
            {
                combatState.EnterCombat(PossibleMobs, exploreLocation, SelectedStorage, isExploring: true, LeaveExploring);
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

        public void SelectExplore(List<ItemDrop> scavengables, List<MobSpawn> mobs, string location, List<Item> locationStorage)
        {
            if (IsExploring)
            {
                LeaveExploring();
                return;
            }
            exploreLocation = location;
            IsExploring = true;
            combatState.LeaveCombat();
            ExploreableItems = scavengables;
            PossibleMobs = new List<MobSpawn>(mobs);
            SurvivalXpAtLocation = 1;
            SelectedStorage = locationStorage;
        }

    }
}
