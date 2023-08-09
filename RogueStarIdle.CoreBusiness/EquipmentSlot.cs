namespace RogueStarIdle.CoreBusiness
{
    public class EquipmentSlot
    {
        public int Id { get; set; }
        public Item? Item { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; } = false;

        public EquipmentSlot(int id, Item item, string name, bool? locked = false)
        {
            Id = id;
            Item = item;
            Name = name;
            Locked = locked ?? false;
        }

    }
}