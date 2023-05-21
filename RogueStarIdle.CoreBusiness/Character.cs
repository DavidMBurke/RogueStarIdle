namespace RogueStarIdle.CoreBusiness
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //*********Skills************
        public int MeleeXp { get; set; } = 0;
        public int RangedXp { get; set; } = 0;
        public int ExplosivesXp { get; set; } = 0;
        public int PsychicXp { get; set; } = 0;
        public int BlockXp { get; set; } = 0;
        public int ArmorCraftingXp { get; set; } = 0;
        public int WeaponSmithingXp { get; set; } = 0;
        public int RoboticsXp { get; set; } = 0;
        public int GeologyXp { get; set; } = 0;
        public int ConstructionXp { get; set; } = 0;
        public int FarmingXp { get; set; } = 0;
        public int ForagingXp { get; set; } = 0;
        public int SurvivalXp { get; set; } = 0;
        public int AnimalsXp { get; set; } = 0;
        public int LeadershipXp { get; set; } = 0;
        public int DiplomacyXp { get; set; } = 0;
        public int StealthXp { get; set; } = 0;
        public int HerbalismXp { get; set; } = 0;
        public int ScrappingXp { get; set; } = 0;
        public int MeleeLevel { get; set; } = 0;
        public int RangedLevel { get; set; } = 0;
        public int ELevellosivesLevel { get; set; } = 0;
        public int PsychicLevel { get; set; } = 0;
        public int BlockLevel { get; set; } = 0;
        public int ArmorCraftingLevel { get; set; } = 0;
        public int WeaponSmithingLevel { get; set; } = 0;
        public int RoboticsLevel { get; set; } = 0;
        public int GeologyLevel { get; set; } = 0;
        public int ConstructionLevel { get; set; } = 0;
        public int FarmingLevel { get; set; } = 0;
        public int ForagingLevel { get; set; } = 0;
        public int SurvivalLevel { get; set; } = 0;
        public int AnimalsLevel { get; set; } = 0;
        public int LeadershipLevel { get; set; } = 0;
        public int DiplomacyLevel { get; set; } = 0;
        public int StealthLevel { get; set; } = 0;
        public int HerbalismLevel { get; set; } = 0;
        public int ScrappingLevel { get; set; } = 0;
        public Miscellaneous miscellaneous = new Miscellaneous();

        public void checkLevel(int level, int Level)
        {
            int checkedLevel = miscellaneous.GetLevelFromXp(Level);
            if (checkedLevel > level)
            {
                level = checkedLevel;
            }
        }
    }
}