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
        public Stats StatBlock { get; set; } = new Stats();

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
            StatBlock = new Stats();
            foreach (var property in typeof(EquipmentSet).GetProperties())
            {
                if (property.PropertyType == typeof(EquipmentSlot))
                {
                    EquipmentSlot slot = (EquipmentSlot)property.GetValue(this);
                    if (slot.Item == null)
                    {
                        continue;
                    }
                    StatBlock.EnergyDefense += slot.Item.EnergyDefense;
                    StatBlock.KineticDefense += slot.Item.KineticDefense;
                    StatBlock.PsychicDefense += slot.Item.PsychicDefense;
                    StatBlock.MeleeDefense += slot.Item.MeleeDefense;
                    StatBlock.RangedDefense += slot.Item.RangedDefense;
                    StatBlock.ExplosiveDefense += slot.Item.ExplosiveDefense;
                    StatBlock.EnergyDR += slot.Item.EnergyDamageReduction;
                    StatBlock.FireDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.FireDamageReduction);
                    StatBlock.AcidDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.AcidDamageReduction);
                    StatBlock.PoisonDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.PoisonDamageReduction);
                    StatBlock.ShockDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.ShockDamageReduction);
                    StatBlock.KineticDR += slot.Item.KineticDamageReduction;
                    StatBlock.PiercingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.PiercingDamageReduction);
                    StatBlock.CrushingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.CrushingDamageReduction);
                    StatBlock.SlashingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.SlashingDamageReduction);
                    StatBlock.PsychicDR += slot.Item.PsychicDamageReduction;
                    StatBlock.FireDamageMin += slot.Item.PercentFireDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.FireDamageMax += slot.Item.PercentFireDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.AcidDamageMin += slot.Item.PercentFireDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.AcidDamageMax += slot.Item.PercentFireDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.PoisonDamageMin += slot.Item.PercentPoisonDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.PoisonDamageMax += slot.Item.PercentPoisonDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.ShockDamageMin += slot.Item.PercentShockDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.ShockDamageMax += slot.Item.PercentShockDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.PiercingDamageMin += slot.Item.PercentPiercingDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.PiercingDamageMax += slot.Item.PercentPiercingDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.SlashingDamageMin += slot.Item.PercentSlashingDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.SlashingDamageMax += slot.Item.PercentSlashingDamage * slot.Item.MaxBaseDamage / 100;
                    StatBlock.CrushingDamageMin += slot.Item.PercentCrushingDamage * slot.Item.MinBaseDamage / 100;
                    StatBlock.CrushingDamageMax += slot.Item.PercentCrushingDamage * slot.Item.MaxBaseDamage / 100;
                }
            }
            if (LeftWeapon.Item != null && RightWeapon.Item != null)
            {
                StatBlock.FireDamageMin = DualWieldPenalty(StatBlock.FireDamageMin);
                StatBlock.FireDamageMax = DualWieldPenalty(StatBlock.FireDamageMax);
                StatBlock.AcidDamageMin = DualWieldPenalty(StatBlock.AcidDamageMin);
                StatBlock.AcidDamageMax = DualWieldPenalty(StatBlock.AcidDamageMax);
                StatBlock.PoisonDamageMin = DualWieldPenalty(StatBlock.PoisonDamageMin);
                StatBlock.PoisonDamageMax = DualWieldPenalty(StatBlock.PoisonDamageMax);
                StatBlock.ShockDamageMin = DualWieldPenalty(StatBlock.ShockDamageMin);
                StatBlock.ShockDamageMax = DualWieldPenalty(StatBlock.ShockDamageMax);
                StatBlock.PiercingDamageMin = DualWieldPenalty(StatBlock.PiercingDamageMin);
                StatBlock.PiercingDamageMax = DualWieldPenalty(StatBlock.PiercingDamageMax);
                StatBlock.SlashingDamageMin = DualWieldPenalty(StatBlock.SlashingDamageMin);
                StatBlock.SlashingDamageMax = DualWieldPenalty(StatBlock.SlashingDamageMax);
                StatBlock.CrushingDamageMin = DualWieldPenalty(StatBlock.CrushingDamageMin);
                StatBlock.CrushingDamageMax = DualWieldPenalty(StatBlock.CrushingDamageMax);
            }
        }
        public int DualWieldPenalty(int damage)
        {
            int reducedDamage = (damage * 60) / 100;
            return reducedDamage;
        }
    }

}