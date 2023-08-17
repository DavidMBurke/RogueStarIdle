namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class SaveState
    {
        public ActionState ActionState { get; set; }
        public CharacterState CharacterState { get; set; }
        public InventoryState InventoryState { get; set; }
        public TimeState TimeState { get; set; }

        public SaveState(ActionState actionState, CharacterState characterState, InventoryState inventoryState, TimeState timeState) { 
            ActionState = actionState;
            CharacterState = characterState;
            InventoryState = inventoryState;
            TimeState = timeState;
        }

        public SaveState() {}
    }
}
