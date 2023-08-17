namespace RogueStarIdle.CoreBusiness
{
    public class Scrap
    {
        public Item Item { get; set; }
        public int QuantityMin { get; set; }
        public int QuantityMax { get; set; }

        //Fraction chance of receiving scrap
        public int Numerator { get; set; }
        public int Denominator { get; set; }

        public Scrap(Item item, int qtyMin, int qtyMax, int numerator, int denominator) {
            Item = item;
            QuantityMin = qtyMin;
            QuantityMax = qtyMax;
            Numerator = numerator;
            Denominator = denominator;
        }
    }
}
