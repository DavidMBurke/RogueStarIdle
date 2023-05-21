namespace RogueStarIdle.CoreBusiness
{
    public class EquipmentSet
    {
        public EquipmentSlot Head { get; set; } = new EquipmentSlot(1, null);
        public EquipmentSlot Neck { get; set; } = new EquipmentSlot(2, null);
        public EquipmentSlot Chest { get; set; } = new EquipmentSlot(3, null);
        public EquipmentSlot Belt { get; set; } = new EquipmentSlot(4, null);
        public EquipmentSlot Legs { get; set; } = new EquipmentSlot(5, null);
        public EquipmentSlot Feet { get; set; } = new EquipmentSlot(6, null);
        public EquipmentSlot Back { get; set; } = new EquipmentSlot(7, null);
        public EquipmentSlot Hands { get; set; } = new EquipmentSlot(8, null);
        public EquipmentSlot LeftWeapon { get; set; } = new EquipmentSlot(9, null);
        public EquipmentSlot LeftWeaponMod1 { get; set; } = new EquipmentSlot(10, null);
        public EquipmentSlot LeftWeaponMod2 { get; set; } = new EquipmentSlot(11, null);
        public EquipmentSlot LeftWeaponMod3 { get; set; } = new EquipmentSlot(12, null);
        public EquipmentSlot RightWeapon { get; set; } = new EquipmentSlot(13, null);
        public EquipmentSlot RightWeaponMod1 { get; set; } = new EquipmentSlot(14, null);
        public EquipmentSlot RightWeaponMod2 { get; set; } = new EquipmentSlot(15, null);
        public EquipmentSlot RightWeaponMod3 { get; set; } = new EquipmentSlot(16, null);
        public EquipmentSlot Stim { get; set; } = new EquipmentSlot(17, null);
        public EquipmentSlot HealthPack { get; set; } = new EquipmentSlot(18, null);
        public EquipmentSlot Explosive { get; set; } = new EquipmentSlot(19, null);
        public EquipmentSlot Droid { get; set; } = new EquipmentSlot(20, null);
        public EquipmentSlot DroidMod { get; set; } = new EquipmentSlot(21, null);
        public EquipmentSlot BrainCybernetic1 { get; set; } = new EquipmentSlot(22, null);
        public EquipmentSlot BrainCybernetic2 { get; set; } = new EquipmentSlot(23, null);
        public EquipmentSlot BrainCybernetic3 { get; set; } = new EquipmentSlot(24, null);
        public EquipmentSlot BodyCybernetic1 { get; set; } = new EquipmentSlot(25, null);
        public EquipmentSlot BodyCybernetic2 { get; set; } = new EquipmentSlot(26, null);
        public EquipmentSlot BodyCybernetic3 { get; set; } = new EquipmentSlot(27, null);
        public EquipmentSlot OmniLinkMod { get; set; } = new EquipmentSlot(28, null);
        public EquipmentStats StatBlock { get; set; } = new EquipmentStats();

        //get name of slot from ID for display purposes
        public string getSlotNameById(int id)
        {
            foreach (var property in typeof(EquipmentSet).GetProperties())
            {
                if (property.PropertyType != typeof(EquipmentSlot))
                {
                    continue;
                }
                EquipmentSlot slot = (EquipmentSlot)property.GetValue(this);
                if (slot.Id != id)
                {
                    continue;
                }
                return property.Name;
                
            }
            return "Error";
        }

        public void CalculateStats()
        {
            StatBlock = new EquipmentStats();
            foreach (var property in typeof(EquipmentSet).GetProperties())
            {
                if (property.PropertyType == typeof(EquipmentSlot))
                {
                    EquipmentSlot slot = (EquipmentSlot)property.GetValue(this);
                    if (slot.Item != null)
                    {
                        StatBlock.minDamage += slot.Item.MinBaseDamage;
                        StatBlock.maxDamage += slot.Item.MaxBaseDamage;
                        StatBlock.energyDefense += slot.Item.EnergyDefense;
                        StatBlock.kineticDefense += slot.Item.KineticDefense;
                        StatBlock.psychicDefense += slot.Item.PsychicDefense;
                        StatBlock.meleeDefense += slot.Item.MeleeDefense;
                        StatBlock.rangedDefense += slot.Item.RangedDefense;
                        StatBlock.explosiveDefense += slot.Item.ExplosiveDefense;
                        StatBlock.energyDR += slot.Item.EnergyDamageReduction;
                        StatBlock.fireDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.FireDamageReduction);
                        StatBlock.acidDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.AcidDamageReduction);
                        StatBlock.poisonDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.PoisonDamageReduction);
                        StatBlock.shockDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.ShockDamageReduction);
                        StatBlock.kineticDR += slot.Item.KineticDamageReduction;
                        StatBlock.piercingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.PiercingDamageReduction);
                        StatBlock.crushingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.CrushingDamageReduction);
                        StatBlock.slashingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.SlashingDamageReduction);
                        StatBlock.psychicDR += slot.Item.PsychicDamageReduction;
                        StatBlock.fireDamage += slot.Item.PercentFireDamage;
                        StatBlock.acidDamage += slot.Item.PercentFireDamage;
                        StatBlock.poisonDamage += slot.Item.PercentPoisonDamage;
                        StatBlock.shockDamage += slot.Item.PercentShockDamage;
                        StatBlock.piercingDamage += slot.Item.PercentPiercingDamage;
                        StatBlock.slashingDamage += slot.Item.PercentSlashingDamage;
                        StatBlock.crushingDamage += slot.Item.PercentCrushingDamage;
                    }
                }
            }
        }
    }
}