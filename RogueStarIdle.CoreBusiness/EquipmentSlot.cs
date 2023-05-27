namespace RogueStarIdle.CoreBusiness
{
    public class EquipmentSlot
    {
        public int Id { get; set; }
        public Item? Item { get; set; }

        public EquipmentSlot(int id, Item item)
        {
            Id = id;
            Item = item;
        }

    }
}