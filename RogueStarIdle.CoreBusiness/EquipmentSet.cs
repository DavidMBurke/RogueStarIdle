using System.Reflection.Metadata;


namespace RogueStarIdle.CoreBusiness
{
    public class EquipmentSet
    {
        public EquipmentSlot Head { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Head, null, "Head");
        public EquipmentSlot Neck { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Neck, null, "Neck");
        public EquipmentSlot Torso { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Torso, null, "Torso");
        public EquipmentSlot Belt { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Belt, null, "Belt");
        public EquipmentSlot Legs { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Legs, null, "Legs");
        public EquipmentSlot Feet { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Feet, null, "Feet");
        public EquipmentSlot Back { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Back, null, "Back");
        public EquipmentSlot Hands { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Hands, null, "Hands");
        public EquipmentSlot LeftWeapon { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.LeftWeapon, null, "Left Weapon");
        public EquipmentSlot LeftWeaponMod1 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.LeftWeaponMod1, null, "Left Weapon Mod 1", locked: true);
        public EquipmentSlot LeftWeaponMod2 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.LeftWeaponMod2, null, "Left Weapon Mod 2", locked: true);
        public EquipmentSlot LeftWeaponMod3 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.LeftWeaponMod3, null, "Left Weapon Mod 3", locked: true);
        public EquipmentSlot RightWeapon { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.RightWeapon, null, "Right Weapon");
        public EquipmentSlot RightWeaponMod1 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.RightWeaponMod1, null, "Right Weapon Mod 1", locked: true);
        public EquipmentSlot RightWeaponMod2 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.RightWeaponMod2, null, "Right Weapon Mod 2", locked: true);
        public EquipmentSlot RightWeaponMod3 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.RightWeaponMod3, null, "Right Weapon Mod 3", locked: true);
        public EquipmentSlot Stim { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Stim, null, "Stim");
        public EquipmentSlot HealthPack { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Aid, null, "Aid");
        public EquipmentSlot Explosive { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Explosive, null, "Explosive");
        public EquipmentSlot Droid { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.Droid, null, "Droid", locked: true);
        public EquipmentSlot DroidMod { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.DroidMod, null, "Droid Mod", locked: true);
        public EquipmentSlot BrainCybernetic1 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BrainCybernetic1, null, "Brain Cybernetic 1", locked: true);
        public EquipmentSlot BrainCybernetic2 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BrainCybernetic2, null, "Brain Cybernetic 2", locked: true);
        public EquipmentSlot BrainCybernetic3 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BrainCybernetic3, null, "Brain Cybernetic 3", locked: true);
        public EquipmentSlot BodyCybernetic1 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BodyCybernetic1, null, "Body Cybernetic 1", locked: true);
        public EquipmentSlot BodyCybernetic2 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BodyCybernetic2, null, "Body Cyberneteic 2", locked: true);
        public EquipmentSlot BodyCybernetic3 { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.BodyCybernetic3, null, "Body Cybernetic 3", locked: true);
        public EquipmentSlot OmniLinkMod { get; set; } = new EquipmentSlot((int)EquipmentSlotEnum.OmniLinkMod, null, "OmniLink Mod");
        public Stats Stats { get; set; } = new Stats();
        public enum EquipmentSlotEnum
        {
            Head = 1,
            Neck = 2,
            Torso = 3,
            Belt = 4,
            Legs = 5,
            Feet = 6,
            Back = 7,
            Hands = 8,
            LeftWeapon = 9,
            LeftWeaponMod1 = 10,
            LeftWeaponMod2 = 11,
            LeftWeaponMod3 = 12,
            RightWeapon = 13,
            RightWeaponMod1 = 14,
            RightWeaponMod2 = 15,
            RightWeaponMod3 = 16,
            Stim = 17,
            Aid = 18,
            Explosive = 19,
            Droid = 20,
            DroidMod = 21,
            BrainCybernetic1 = 22,
            BrainCybernetic2 = 23,
            BrainCybernetic3 = 24,
            BodyCybernetic1 = 25,
            BodyCybernetic2 = 26,
            BodyCybernetic3 = 27,
            OmniLinkMod = 28
        }

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
                MaxHealth = 10 * character.VitalitySkill.Level + GetTotalLevel(character)
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