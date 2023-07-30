namespace RogueStarIdle.CoreBusiness
{
    public class Mob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Stats Stats { get; set; } = new Stats();
        public List<ItemDrop> Loot { get; set; } = new List<ItemDrop>();
        public void Attack(Character defender)
        {
            if (defender == null)
            {
                return;
            }
            Console.WriteLine($"Player attacks {defender.Name}!");
            Random rand = new Random();
            int hitRoll = rand.Next(20);
            int blockRoll = defender.Equipment.Stats.MeleeDefense + rand.Next(20);
            if (hitRoll > blockRoll)
            {
                // TODO create method to total all damages and subtract all defenses
                int damage = CalculateTotalDamage(Stats, defender.Equipment.Stats);
                defender.Equipment.Stats.CurrentHealth -= damage;
                Console.WriteLine($"{Name} hits for {damage} damage! {defender.Name} HP: ({defender.Equipment.Stats.CurrentHealth}/{defender.Equipment.Stats.MaxHealth})");
            }
            else
            {
                Console.WriteLine($"{Name} misses!");
            }
        }

        public int CalculateTotalDamage(Stats attacker, Stats defender)
        {
            Random rand = new Random();
            int damage = 0;
            damage += CalculateDamageByType(attacker.PiercingDamageMin, attacker.PiercingDamageMax, defender.PiercingDR);
            damage += CalculateDamageByType(attacker.SlashingDamageMin, attacker.SlashingDamageMax, defender.SlashingDR);
            damage += CalculateDamageByType(attacker.CrushingDamageMin, attacker.CrushingDamageMax, defender.CrushingDR);
            damage += CalculateDamageByType(attacker.AcidDamageMin, attacker.AcidDamageMax, defender.AcidDR);
            damage += CalculateDamageByType(attacker.FireDamageMin, attacker.FireDamageMax, defender.FireDR);
            damage += CalculateDamageByType(attacker.ShockDamageMin, attacker.ShockDamageMax, defender.ShockDR);
            damage += CalculateDamageByType(attacker.PoisonDamageMin, attacker.PoisonDamageMax, defender.PoisonDR);
            return damage;
        }

        public int CalculateDamageByType(int min, int max, int dr)
        {
            Random rand = new Random();
            int baseDamage = min + rand.Next(1 + max - min) * dr;
            int reducedDamage = (baseDamage * (100 - dr)) / 100;
            return reducedDamage;
        }
    }
}