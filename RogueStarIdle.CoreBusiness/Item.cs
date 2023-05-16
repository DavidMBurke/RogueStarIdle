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
        // Equipment slot IDs that are allowed
        public List<int> EquipmentSlots { get; set; } = new List<int>();
        public bool IsWeapon { get; set; } = false;
        public bool IsArmor { get; set; } = false;

        /// *************SPECIFIC TO WEAPONS****************

        // stores Item IDs of ammo types that are allowed to be used with weapon
        public List<int> AllowedAmmo { get; set; } = new List<int>();
        public bool IsMelee { get; set; } = false;
        public bool IsRanged { get; set; } = false;
        public bool IsExplosive { get; set; } = false; 
        public int ToHitModifier { get; set; } = 0;
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

    // ************CONSUMABLES**************


    Item createComponent(int id, string name, int quantity, int buyPrice, int sellPrice, List<string> tags)
        {
            Item component = new Item
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                Tags = tags
            };
            return component;
        }

    Item createWeapon(int id, string name, int quantity, int buyPrice, int sellPrice, List<string> tags, List<int> equipmentSlots, int toHitModifier, int minBaseDamage, int maxBaseDamage,
        List<int> allowedAmmo = null, bool isMelee = false, bool isRanged = false, bool isExplosive = false, int percentAcidDamage = 0, int percentFireDamage = 0, 
        int percentPoisonDamage = 0, int percentShockDamage = 0, int percentPiercingDamage = 0, int percentSlashingDamage = 0, int percentCrushingDamage = 0, 
        int qualityLevel = 0, int maxQualityLevel = 5)
        {
            Item weapon = new Item
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                Tags = tags,
                IsEquippable = true,
                EquipmentSlots = equipmentSlots,
                IsWeapon = true,
                AllowedAmmo = allowedAmmo ?? new List<int>(),
                IsMelee = isMelee,
                IsRanged = isRanged,
                IsExplosive = isExplosive,
                ToHitModifier = toHitModifier,
                MinBaseDamage= minBaseDamage,
                MaxBaseDamage = maxBaseDamage,
                PercentAcidDamage = percentAcidDamage,
                PercentFireDamage = percentFireDamage,
                PercentPoisonDamage = percentPoisonDamage,
                PercentShockDamage = percentShockDamage,
                PercentCrushingDamage = percentCrushingDamage,
                PercentPiercingDamage = percentPiercingDamage,
                PercentSlashingDamage = percentSlashingDamage,
                QualityLevel = qualityLevel,
                MaxQualityLevel = maxQualityLevel
            };
            return weapon;
        }

        Item createArmor(int id, string name, int quantity, int buyPrice, int sellPrice, List<string> tags, List<int> equipmentSlots, int meleeDef = 0, int rangedDef = 0, int explosivesDef = 0, 
            int kineticDef = 0, int energyDef = 0, int psychicDef = 0, int psychicDR = 0, int kineticDR = 0, int piercingDR = 0, int slashingDR = 0, int crushingDR = 0,
            int energyDR = 0, int fireDR = 0, int poisonDR = 0, int acidDR = 0, int shockDR = 0, int qualityLevel = 0, int maxQualityLevel = 0)
        {
            Item component = new Item
            {
                Id = id,
                Name = name,
                Quantity = quantity,
                BuyPrice = buyPrice,
                SellPrice = sellPrice,
                Tags = tags,
                IsEquippable = true,
                EquipmentSlots = equipmentSlots,
                IsArmor = true,
                MeleeDefense = meleeDef,
                RangedDefense = rangedDef,
                ExplosiveDefense = explosivesDef,
                KineticDefense = kineticDef,
                EnergyDefense = energyDef,
                PsychicDefense = psychicDef,
                PsychicDamageReduction = psychicDR,
                KineticDamageReduction = kineticDR,
                PiercingDamageReduction = (piercingDR > kineticDR) ? piercingDR : kineticDR,
                SlashingDamageReduction = (slashingDR > kineticDR) ? slashingDR : kineticDR,
                CrushingDamageReduction = (crushingDR > kineticDR) ? crushingDR : kineticDR,
                EnergyDamageReduction = energyDR,
                FireDamageReduction = (fireDR > energyDR) ? fireDR : energyDR,
                AcidDamageReduction = (acidDR > energyDR) ? acidDR : energyDR,
                PoisonDamageReduction = (poisonDR > energyDR) ? poisonDR : energyDR,
                ShockDamageReduction = (shockDR > energyDR) ? shockDR : energyDR,
                QualityLevel = qualityLevel,
                MaxQualityLevel = maxQualityLevel


            };
            return component;
        }

    }


}