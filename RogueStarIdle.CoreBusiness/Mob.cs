namespace RogueStarIdle.CoreBusiness
{
    public class Mob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Stats Stats { get; set; } = new Stats();
        public List<ItemDrop> Loot { get; set; } = new List<ItemDrop>();
        public int CurrentHealth { get; set; } = 0;
        public bool IsAlive { get; set; } = true;
        public string Image { get; set; } = "";
        public void Attack(Character defender)
        {
            if (defender == null)
            {
                return;
            }
            Console.WriteLine($"{Name} attacks player!");
            Random rand = new Random();
            int hitRoll = rand.Next(20);
            int blockRoll = defender.Equipment.Stats.MeleeDefense + defender.BlockSkill.Level + rand.Next(20);
            int damage = CalculateTotalDamage(Stats, defender.Equipment.Stats);
            if (hitRoll > blockRoll)
            {
                defender.CurrentHealth -= damage;
                defender.VitalitySkill.addXp(1);
                Console.WriteLine($"{Name} hits for {damage} damage! {defender.Name} HP: ({defender.CurrentHealth}/{defender.Equipment.Stats.MaxHealth})");
            }
            else
            {
                defender.BlockSkill.addXp(1);
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
            int baseDamage = min + rand.Next(1 + max - min);
            int reducedDamage = (baseDamage * (100 - dr)) / 100;
            return reducedDamage;
        }

        public void Die()
        {
            IsAlive = false;
        }

        public void Revive()
        { 
            IsAlive = true; 
        }
    }
}