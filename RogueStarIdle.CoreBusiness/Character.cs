using static RogueStarIdle.CoreBusiness.CombatRule;

namespace RogueStarIdle.CoreBusiness
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public EquipmentSet Equipment { get; set; } = new EquipmentSet();
        public int ActionCounter { get; set; }
        public int CurrentHealth { get; set; } = 0;
        public List<CombatRule> CombatRules { get; set; } = new List<CombatRule>()
        {
            new CombatRule()
        };
        public bool IsAlive { get; set; } = true;   
        public float PassiveHealingTracker { get; set; } = 0;
        public bool TriggerAttackAnimation { get; set; } = false;
        public Skill MeleeSkill { get; set; } = new Skill("Melee", 1, 0);
        public Skill RangedSkill { get; set; } = new Skill("Ranged", 1, 0);
        public Skill ExplosivesSkill { get; set; } = new Skill("Explosives", 1, 0);
        public Skill PsychicSkill { get; set; } = new Skill("Psychic", 1, 0);
        public Skill BlockSkill { get; set; } = new Skill("Block", 1, 0);
        public Skill VitalitySkill { get; set; } = new Skill("Vitality", 1, 0);
        public Skill ArmorCraftingSkill { get; set; } = new Skill("Armor Crafting", 1, 0);
        public Skill WeaponCraftingSkill { get; set; } = new Skill("Weapon Crafting", 1, 0);
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
        public Skill MedicineSkill { get; set; } = new Skill("Medicine", 1, 0);
        public Skill ScrappingSkill { get; set; } = new Skill("Scrapping", 1, 0);
        public CharacterImageUrls Images { get; set; } = new CharacterImageUrls();

        public void TakeAction(Character main, List<Character> allies, List<MobSpawn> enemies)
        {
            bool actionTaken = false;
            foreach (CombatRule rule in CombatRules)
            {
                switch(rule.Action)
                {
                    case (int)ActionEnum.Attack: actionTaken = Attack(enemies, rule); break;
                }
                if (actionTaken) { break; }
            }
        }

        public bool Attack(List<MobSpawn> defenders, CombatRule rule)
        {
            if (defenders == null)
            {
                return false;
            }
            List<MobSpawn> aliveDefenders = defenders.Where(d => d.Mob.IsAlive).ToList();
            if (aliveDefenders.Count == 0)
            {
                return false;
            }
            TriggerAttackAnimation = true;

            Random rand = new Random();
            MobSpawn defender = new MobSpawn();

            switch (rule.TargetQualifier)
            {
                case (int)TargetQualifierEnum.Any:
                    {
                        defender = aliveDefenders.First();
                        break;
                    }
                case (int)TargetQualifierEnum.Lowest:
                    {
                        defender = getDefenderByStat(aliveDefenders, rule.TargetQualifierStat, "Lowest");
                        break;
                    }
                case (int)TargetQualifierEnum.Highest:
                    {
                        defender = getDefenderByStat(aliveDefenders, rule.TargetQualifierStat);
                        break;
                    }
            }

            // TODO: Add Conditionals

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

            int xp = 1;
            int blockRoll = defender.Mob.Stats.MeleeDefense + rand.Next(20);
            if (hitRoll > blockRoll)
            {
                int damage = CalculateTotalDamage(Equipment.Stats, defender.Mob.Stats);
                defender.Mob.CurrentHealth -= damage;
                xp = 2;
            }
            if (Equipment.Stats.IsUsingMelee)
            {
                MeleeSkill.addXp(xp);
            }
            if (Equipment.Stats.IsUsingRanged)
            {
                RangedSkill.addXp(xp);
            }
            if (Equipment.Stats.IsUsingExplosive)
            {
                ExplosivesSkill.addXp(xp);
            }
            if (Equipment.Stats.IsUsingPsychic)
            {
                PsychicSkill.addXp(xp);
            }
            return true;
        }

        private MobSpawn getDefenderByStat(List<MobSpawn> defenders, int targetQualifierStat, string sortBy = "")
        {
            if (sortBy == "Lowest")
            {
                switch(targetQualifierStat)
                {
                    case (int)StatDropdownEnum.CurrentHealth: 
                        return defenders.OrderBy(d => d.Mob.CurrentHealth).First();
                    case (int)StatDropdownEnum.MaxHealth: 
                        return defenders.OrderBy(d => d.Mob.Stats.MaxHealth).First();
                    case (int)StatDropdownEnum.MeleeToHit: 
                        return defenders.OrderBy(d => d.Mob.Stats.MeleeToHit).First();
                    case (int)StatDropdownEnum.MeleeDamageMax: 
                        return defenders.OrderBy(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.RangedToHit: 
                        return defenders.OrderBy(d => d.Mob.Stats.RangedToHit).First();
                    case (int)StatDropdownEnum.RangedDamageMax:
                        return defenders.OrderBy(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                    case (int)StatDropdownEnum.PsychicToHit: 
                        return defenders.OrderBy(d => d.Mob.Stats.PsychicToHit).First();
                    case (int)StatDropdownEnum.ExplosiveToHit: 
                        return defenders.OrderBy(d => d.Mob.Stats.ExplosiveToHit).First();
                    default:
                        return defenders.OrderBy(d => d.Mob.CurrentHealth).First();
                }
            }
            switch (targetQualifierStat)
            {
                case (int)StatDropdownEnum.CurrentHealth:
                    return defenders.OrderByDescending(d => d.Mob.CurrentHealth).First();
                case (int)StatDropdownEnum.MaxHealth:
                    return defenders.OrderByDescending(d => d.Mob.Stats.MaxHealth).First();
                case (int)StatDropdownEnum.MeleeToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.MeleeToHit).First();
                case (int)StatDropdownEnum.MeleeDamageMax:
                    return defenders.OrderByDescending(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.RangedToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.RangedToHit).First();
                case (int)StatDropdownEnum.RangedDamageMax:
                    return defenders.OrderByDescending(d => new[] { d.Mob.Stats.CrushingDamageMax, d.Mob.Stats.SlashingDamageMax, d.Mob.Stats.PiercingDamageMax }.Max()).First();
                case (int)StatDropdownEnum.PsychicToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.PsychicToHit).First();
                case (int)StatDropdownEnum.ExplosiveToHit:
                    return defenders.OrderByDescending(d => d.Mob.Stats.ExplosiveToHit).First();
                default:
                    return defenders.OrderByDescending(d => d.Mob.CurrentHealth).First();
            }
        }

        int GetMax (List<int> ints)
        {
            return ints.OrderBy(i => i).Max();
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

        public void PassiveHeal(int timeToFullHealth)
        {
            if (CurrentHealth >= Equipment.Stats.MaxHealth || IsAlive == false)
            {
                return;
            }
            PassiveHealingTracker += (float)Equipment.Stats.MaxHealth / timeToFullHealth;
            while (PassiveHealingTracker > 1 && CurrentHealth < Equipment.Stats.MaxHealth)
            {
                CurrentHealth++;
                PassiveHealingTracker--;
            }
            if (CurrentHealth >= Equipment.Stats.MaxHealth)
            {
                PassiveHealingTracker = 0;
            }
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