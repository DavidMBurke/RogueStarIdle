namespace RogueStarIdle.CoreBusiness
{
    public class MainCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //********Combat stats*************
        public int toHitModifier { get; set; } = 0;
        public int minBaseDamage { get; set; } = 0;
        public int maxBaseDamage { get; set; } = 0;

        // damage percentages applied to base damage and do not need to add to 100%. Displayed damage range will factor in sum of percentages
        // Fire, poison, acid, and shock all under 'energy', so buffs/debuffs that apply to 'energy' apply to all of these
        public int percentFireDamage { get; set; } = 0;
        public int percentPoisonDamage { get; set; } = 0;
        public int percentAcidDamage { get; set; } = 0;
        public int percentShockDamage { get; set; } = 0;

        // Piercing, slashing and crushing all under 'kinetic', so buffs/debuffs that apply to 'kinetic' apply to all of these
        public int percentPiercingDamage { get; set; } = 0;
        public int percentSlashingDamage { get; set; } = 0;
        public int percentCrushingDamage { get; set; } = 0;

        //*********Skills************
        public int MeleeXp { get; set; } = 0;
        public int RangedXp { get; set; } = 0;
        public int ExplosivesXp { get; set; } = 0;
        public int PsychicXp { get; set; } = 0;
        public int Block { get; set; } = 0;
        public int ArmorCrafting { get; set; } = 0;
        public int WeaponSmithing { get; set; } = 0;
        public int Robotics { get; set; } = 0;
        public int Geology { get; set; } = 0;
        public int Construction { get; set; } = 0;
        public int Farming { get; set; } = 0;
        public int Foraging { get; set; } = 0;
        public int Survival { get; set; } = 0;
        public int Animals { get; set; } = 0;
        public int Leadership { get; set; } = 0;
        public int Diplomacy { get; set; } = 0;
        public int Stealth { get; set; } = 0;
        public int Herbalism { get; set; } = 0;
        public int Scrapping { get; set; } = 0;

        public int CalculateLevel (int xp)
        {
            //Derived from xp formula of requiredXp = previousLevelReqXP * 1.05 + 500 * prevLevel
            double level = Math.Pow((1.05), (1 - xp)) + 200000 * Math.Pow(1.05, xp - 1) - 10000 * xp;
            return ((int)level);
        }
    }
}