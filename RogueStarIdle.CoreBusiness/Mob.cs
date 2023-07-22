namespace RogueStarIdle.CoreBusiness
{
    public class Mob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Stats Stats { get; set; } = new Stats();
    }
}