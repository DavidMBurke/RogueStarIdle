namespace RogueStarIdle.CoreBusiness
{
    public class MobSpawn
    {
        public Mob Mob { get; set; } = new Mob();
        public List<ItemDrop> Loot { get; set; }
        public int SpawnChance { get; set; } //This will be numerator with denom being total of all mob spawn chances in area
        public MobSpawn (Mob mob, int spawnChance, List<ItemDrop>? loot = null)
        {
            Mob = mob;
            SpawnChance = spawnChance;
            Loot = loot ?? mob.Loot;
        }
    }
}