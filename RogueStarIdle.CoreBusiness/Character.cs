namespace RogueStarIdle.CoreBusiness
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public EquipmentSet Equipment { get; set; } = new EquipmentSet();
        public int AttackCounter { get; set; }
        public int CurrentHealth { get; set; } = 0;
        public Skill MeleeSkill { get; set; } = new Skill("Melee", 1, 0);
        public Skill RangedSkill { get; set; } = new Skill("Ranged", 1, 0);
        public Skill ExplosivesSkill { get; set; } = new Skill("Explosives", 1, 0);
        public Skill PsychicSkill { get; set; } = new Skill("Psychic", 1, 0);
        public Skill BlockSkill { get; set; } = new Skill("Block", 1, 0);
        public Skill ArmorCraftingSkill { get; set; } = new Skill("Armor Crafting", 1, 0);
        public Skill WeaponSmithingSkill { get; set; } = new Skill("Weapon Smithing", 1, 0);
        public Skill RoboticsSkill { get; set; } = new Skill("Robotics", 1, 0);
        public Skill GeologySkill { get; set; } = new Skill("Geology", 1, 0);
        public Skill ConstructionSkill { get; set; } = new Skill("Construction", 1, 0);
        public Skill FarmingSkill { get; set; } = new Skill("Farming", 1, 0);
        public Skill ForagingSkill { get; set; } = new Skill("Foraging", 1, 0);
        public Skill SurvivalSkill { get; set; } = new Skill("Survival", 1, 0);
        public Skill AnimalsSkill { get; set; } = new Skill("Animals", 1, 0);
        public Skill LeadershipSkill { get; set; } = new Skill("Leadership", 1, 0);
        public Skill DiplomacySkill { get; set; } = new Skill("Diplomacy", 1, 0);
        public Skill StealthSkill { get; set; } = new Skill("Stealth", 1, 0);
        public Skill HerbalismSkill { get; set; } = new Skill("Herbalism", 1, 0);
        public Skill ScrappingSkill { get; set; } = new Skill("Scrapping", 1, 0);
        public string Image = "";

        public void Attack(MobSpawn defender)
        {
            if (defender == null)
            {
                return;
            }
            Console.WriteLine($"Player attacks {defender.Mob.Name}!");
            Random rand = new Random();
            int hitRoll = rand.Next(20);
            if (Equipment.Stats.IsUsingMelee)
            {
                hitRoll += Equipment.Stats.MeleeToHit;
            }
            if (Equipment.Stats.IsUsingRanged)
            {
                hitRoll += Equipment.Stats.RangedToHit;
            }
            if (Equipment.Stats.IsUsingExplosive)
            {
                hitRoll += Equipment.Stats.ExplosiveToHit;
            }
            if (Equipment.Stats.IsUsingPsychic)
            {
                hitRoll += Equipment.Stats.PsychicToHit;
            }

            int blockRoll = defender.Mob.Stats.MeleeDefense + rand.Next(20);
            if (hitRoll > blockRoll)
            {
                // TODO create method to total all damages and subtract all defenses
                int damage = CalculateTotalDamage(Equipment.Stats, defender.Mob.Stats);
                defender.Mob.CurrentHealth -= damage;
                Console.WriteLine($"Player hits for {damage} damage! {defender.Mob.Name} HP: ({defender.Mob.CurrentHealth}/{defender.Mob.Stats.MaxHealth})");
            } else
            {
                Console.WriteLine("Player misses!");
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
    }
}