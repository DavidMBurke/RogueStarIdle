namespace RogueStarIdle.CoreBusiness
{
    public class Item
    {
        //Note: All numerical values in item represent base value before outside modifiers applied
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public int BuyPrice { get; set; } = 0;
        public int SellPrice { get; set; } = 0;
        // Tags for search and filters
        public List<string> Tags { get; set; } = new List<string>();
        public bool IsEquippable { get; set; } = false;
        public bool Equipped { get; set; } = false;
        // Equipment slot IDs that are allowed
        public List<int> EquipmentSlots { get; set; } = new List<int>();
        public bool StacksInEquipmentSlot { get; set; } = false;
        public bool IsWeapon { get; set; } = false;
        public bool IsArmor { get; set; } = false;

        /// *************SPECIFIC TO WEAPONS****************

        // stores Item IDs of ammo types that are allowed to be used with weapon
        public int AttackSpeed { get; set; } = 0;
        public List<int> AllowedAmmo { get; set; } = new List<int>();
        public bool IsMelee { get; set; } = false;
        public bool IsRanged { get; set; } = false;
        public bool IsExplosive { get; set; } = false;
        public bool IsPsychic { get; set; } = false;
        public int MeleeToHit { get; set; } = 0;
        public int RangedToHit { get; set; } = 0;
        public int PsychicToHit { get; set; } = 0;
        public int ExplosiveToHit { get; set; } = 0;
        // Base damage never actually used directly, but will be calculated when adding damage type totals to equipment stats
        public int MinBaseDamage { get; set; } = 0;
        public int MaxBaseDamage { get; set; } = 0;
        // damage percentages applied to base damage and do not need to add to 100%. Displayed damage range will factor in sum of percentages
        // Fire, poison, acid, and shock all under 'energy', so buffs/debuffs that apply to 'energy' apply to all of these
        public int PercentFireDamage { get; set; } = 0;
        public int PercentPoisonDamage { get; set; } = 0;
        public int PercentAcidDamage { get; set; } = 0;
        public int PercentShockDamage { get; set; } = 0;
        // Piercing, slashing and crushing all under 'kinetic', so buffs/debuffs that apply to 'kinetic' apply to all of these
        public int PercentPiercingDamage { get; set; } = 0;
        public int PercentSlashingDamage { get; set; } = 0;
        public int PercentCrushingDamage { get; set; } = 0;

        // *************WEAPONS AND ARMOR**************
        public int QualityLevel { get; set; } = 0;
        public int MaxQualityLevel { get; set; } = 0;

        // *************SPECIFIC TO ARMOR***************
        // Higher Defense is lower chance to hit, Higher Damage Reduction is amount damage reduced by
        public int MeleeDefense { get; set; } = 0;
        public int RangedDefense { get; set; } = 0;
        public int ExplosiveDefense { get; set; } = 0;
        public int KineticDefense { get; set; } = 0;
        public int EnergyDefense { get; set; } = 0;
        public int PsychicDefense { get; set; } = 0;
        public int PsychicDamageReduction { get; set; } = 0;
        public int KineticDamageReduction { get; set; } = 0;
        public int PiercingDamageReduction { get; set; } = 0;
        public int SlashingDamageReduction { get; set; } = 0;
        public int CrushingDamageReduction { get; set; } = 0;
        public int EnergyDamageReduction { get; set; } = 0;
        public int FireDamageReduction { get; set; } = 0;
        public int PoisonDamageReduction { get; set; } = 0;
        public int AcidDamageReduction { get; set; } = 0;
        public int ShockDamageReduction { get; set; } = 0;
        public string Thumbnail { get; set; } = "";
        public ImageUrls Images { get; set; } = new ImageUrls();
        // AuxImages for cases where more than one body part animation covered by same equipment, ex: Tops covering torso and both arms
        public ImageUrls AuxImages1 { get; set; } = new ImageUrls();
        public ImageUrls AuxImages2 { get; set; } = new ImageUrls();
        // ************CONSUMABLES**************
        public bool Consumable { get; set; } = false;
        public int HealthRestored { get; set; } = 0;

        public Item CreateCopy()
        {
            return (Item)MemberwiseClone();
        }
    }
}