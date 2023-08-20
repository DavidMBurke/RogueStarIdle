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