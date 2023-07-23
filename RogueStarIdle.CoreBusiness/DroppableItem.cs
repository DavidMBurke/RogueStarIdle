namespace RogueStarIdle.CoreBusiness
{
    public class DroppableItem
    {
        public Item Item { get; set; } = new Item();
        public int DropChanceNumerator { get; set; }
        public int DropChanceDenominator { get;set; }
        public int QuantityRangeMin { get; set; }
        public int QuantityRangeMax { get; set;}
        public DroppableItem (Item item, int dropChanceNumerator, int dropChanceDenomenator, int qtyRangeMin, int qtyRangeMax)
        {
            Item = item;
            DropChanceNumerator = dropChanceNumerator;
            DropChanceDenominator = dropChanceDenomenator;
            QuantityRangeMin = qtyRangeMin;
            QuantityRangeMax = qtyRangeMax;
        }
    }
}