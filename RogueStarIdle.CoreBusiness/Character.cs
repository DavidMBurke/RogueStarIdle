namespace RogueStarIdle.CoreBusiness
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
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
    }
}