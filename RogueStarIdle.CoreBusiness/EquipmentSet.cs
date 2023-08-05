using System.Reflection.Metadata;

namespace RogueStarIdle.CoreBusiness
{
    public class EquipmentSet
    {
        public EquipmentSlot Head { get; set; } = new EquipmentSlot(1, null);
        public EquipmentSlot Neck { get; set; } = new EquipmentSlot(2, null);
        public EquipmentSlot Torso { get; set; } = new EquipmentSlot(3, null);
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
        public Stats Stats { get; set; } = new Stats();

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

        public void CalculateStats(Character character)
        {
            Stats = new Stats()
            {
                MeleeToHit = character.MeleeSkill.Level,
                RangedToHit = character.RangedSkill.Level,
                PsychicToHit = character.PsychicSkill.Level,
                ExplosiveToHit = character.ExplosivesSkill.Level,
                MaxHealth = GetTotalLevel(character)
            };
            foreach (var property in typeof(EquipmentSet).GetProperties())
            {
                if (property.PropertyType == typeof(EquipmentSlot))
                {
                    EquipmentSlot slot = (EquipmentSlot)property.GetValue(this);
                    if (slot.Item == null)
                    {
                        continue;
                    }
                    Stats.MeleeToHit += slot.Item.MeleeToHit;
                    Stats.RangedToHit += slot.Item.RangedToHit;
                    Stats.PsychicToHit += slot.Item.PsychicToHit;
                    Stats.ExplosiveToHit += slot.Item.ExplosiveToHit;
                    Stats.EnergyDefense += slot.Item.EnergyDefense;
                    Stats.KineticDefense += slot.Item.KineticDefense;
                    Stats.PsychicDefense += slot.Item.PsychicDefense;
                    Stats.MeleeDefense += slot.Item.MeleeDefense;
                    Stats.RangedDefense += slot.Item.RangedDefense;
                    Stats.ExplosiveDefense += slot.Item.ExplosiveDefense;
                    Stats.EnergyDR += slot.Item.EnergyDamageReduction;
                    Stats.FireDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.FireDamageReduction);
                    Stats.AcidDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.AcidDamageReduction);
                    Stats.PoisonDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.PoisonDamageReduction);
                    Stats.ShockDR += Math.Max(slot.Item.EnergyDamageReduction, slot.Item.ShockDamageReduction);
                    Stats.KineticDR += slot.Item.KineticDamageReduction;
                    Stats.PiercingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.PiercingDamageReduction);
                    Stats.CrushingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.CrushingDamageReduction);
                    Stats.SlashingDR += Math.Max(slot.Item.KineticDamageReduction, slot.Item.SlashingDamageReduction);
                    Stats.PsychicDR += slot.Item.PsychicDamageReduction;
                    Stats.FireDamageMin += slot.Item.PercentFireDamage * slot.Item.MinBaseDamage / 100;
                    Stats.FireDamageMax += slot.Item.PercentFireDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.AcidDamageMin += slot.Item.PercentFireDamage * slot.Item.MinBaseDamage / 100;
                    Stats.AcidDamageMax += slot.Item.PercentFireDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.PoisonDamageMin += slot.Item.PercentPoisonDamage * slot.Item.MinBaseDamage / 100;
                    Stats.PoisonDamageMax += slot.Item.PercentPoisonDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.ShockDamageMin += slot.Item.PercentShockDamage * slot.Item.MinBaseDamage / 100;
                    Stats.ShockDamageMax += slot.Item.PercentShockDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.PiercingDamageMin += slot.Item.PercentPiercingDamage * slot.Item.MinBaseDamage / 100;
                    Stats.PiercingDamageMax += slot.Item.PercentPiercingDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.SlashingDamageMin += slot.Item.PercentSlashingDamage * slot.Item.MinBaseDamage / 100;
                    Stats.SlashingDamageMax += slot.Item.PercentSlashingDamage * slot.Item.MaxBaseDamage / 100;
                    Stats.CrushingDamageMin += slot.Item.PercentCrushingDamage * slot.Item.MinBaseDamage / 100;
                    Stats.CrushingDamageMax += slot.Item.PercentCrushingDamage * slot.Item.MaxBaseDamage / 100;
                }
            }
            SetAttackType(LeftWeapon.Item, RightWeapon.Item);
            if (LeftWeapon.Item != null && RightWeapon.Item != null)
            {
                Stats.FireDamageMin = DualWieldPenalty(Stats.FireDamageMin);
                Stats.FireDamageMax = DualWieldPenalty(Stats.FireDamageMax);
                Stats.AcidDamageMin = DualWieldPenalty(Stats.AcidDamageMin);
                Stats.AcidDamageMax = DualWieldPenalty(Stats.AcidDamageMax);
                Stats.PoisonDamageMin = DualWieldPenalty(Stats.PoisonDamageMin);
                Stats.PoisonDamageMax = DualWieldPenalty(Stats.PoisonDamageMax);
                Stats.ShockDamageMin = DualWieldPenalty(Stats.ShockDamageMin);
                Stats.ShockDamageMax = DualWieldPenalty(Stats.ShockDamageMax);
                Stats.PiercingDamageMin = DualWieldPenalty(Stats.PiercingDamageMin);
                Stats.PiercingDamageMax = DualWieldPenalty(Stats.PiercingDamageMax);
                Stats.SlashingDamageMin = DualWieldPenalty(Stats.SlashingDamageMin);
                Stats.SlashingDamageMax = DualWieldPenalty(Stats.SlashingDamageMax);
                Stats.CrushingDamageMin = DualWieldPenalty(Stats.CrushingDamageMin);
                Stats.CrushingDamageMax = DualWieldPenalty(Stats.CrushingDamageMax);
                Stats.AttackSpeed = (LeftWeapon.Item.AttackSpeed + RightWeapon.Item.AttackSpeed) / 2;
                //Subtract average of previously added toHit of weapons so as to take average but not cancel bonuses from other equipment
                Stats.MeleeToHit -= (LeftWeapon.Item.MeleeToHit + RightWeapon.Item.MeleeToHit) / 2;
                Stats.RangedToHit -= (LeftWeapon.Item.RangedToHit + RightWeapon.Item.RangedToHit) / 2;
            }
            if (LeftWeapon.Item == null && RightWeapon.Item == null)
            {
                Stats.AttackSpeed = 100; // 2 sec
                Stats.IsUsingMelee = true;
                Stats.CrushingDamageMin = 0;
                Stats.CrushingDamageMax = character.MeleeSkill.Level;
            }
            if (LeftWeapon.Item == null && RightWeapon.Item != null) {
                Stats.AttackSpeed = RightWeapon.Item.AttackSpeed;
            }
            if (LeftWeapon.Item != null && RightWeapon.Item == null) {
                Stats.AttackSpeed = LeftWeapon.Item.AttackSpeed;
            }
        }
        public int DualWieldPenalty(int damage)
        {
            int reducedDamage = (damage * 60) / 100;
            return reducedDamage;
        }

        public void SetAttackType(Item? leftWeapon = null, Item? rightWeapon = null)
        {
            Stats.IsUsingMelee = (leftWeapon?.IsMelee ?? false) || (rightWeapon?.IsMelee?? false);
            Stats.IsUsingRanged = (leftWeapon?.IsRanged?? false) || (rightWeapon?.IsRanged?? false);
            Stats.IsUsingPsychic = (leftWeapon?.IsPsychic?? false) || (rightWeapon?.IsPsychic?? false);
            Stats.IsUsingExplosive = (leftWeapon?.IsExplosive?? false) || (rightWeapon?.IsExplosive?? false);
        }

        public int GetTotalLevel(Character character)
        {
            int totalLevel = 0;
            foreach (var property in typeof(Character).GetProperties())
            {
                if (property.PropertyType == typeof(Skill))
                {
                    Skill skill = (Skill)property.GetValue(character);
                    totalLevel += skill.Level;
                }
            }
            return totalLevel;
        }
    }

}