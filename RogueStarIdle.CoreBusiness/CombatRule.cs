using static RogueStarIdle.CoreBusiness.CombatRule;

namespace RogueStarIdle.CoreBusiness
{
    public class CombatRule
    {
        public int Action { get; set; } = (int)ActionEnum.Attack;
        public int Target { get; set; } = (int)TargetEnum.Enemy;
        public int TargetQualifier { get; set; } = (int)TargetQualifierEnum.Lowest;
        public int TargetQualifierStat { get; set; } = (int)StatDropdownEnum.CurrentHealth;
        public bool IsConditional { get; set; } = false;
        public int ConditionalTarget { get; set; } = (int)TargetEnum.Self;
        public int ConditionalTargetRule { get; set; } = (int)ConditionalTargetRuleEnum._;
        public int ConditionalTargetComparison { get; set; } = (int)ComparisonEnum.LessThan;
        public int ConditionalTargetStat { get; set; } = (int)TargetQualifierEnum.Any;
        public int Percentage { get; set; } = (int)PercentageEnum._0;
        public int ConditionalTargetCount { get; set; } = (int)ConditionalTargetCountEnum._1;

        public enum ActionEnum
        {
            Run = 0,
            Attack = 1,
            Heal = 2,
            ThrowExplosive = 3
        }

        public enum TargetEnum
        {
            Self = 0,
            Ally = 1,
            Enemy = 2
        }

        public enum TargetQualifierEnum
        {
            Any = 0,
            Highest = 1,
            Lowest = 2
        }
        public enum StatDropdownEnum
        {
            CurrentHealth = 0,
            MaxHealth = 1,
            MeleeToHit = 2,
            MeleeDamageMax = 3,
            RangedToHit = 4,
            RangedDamageMax = 5,
            PsychicToHit = 6,
            ExplosiveToHit = 7
        }

        public enum ComparisonEnum
        {
            LessThan = 0,
            MoreThan = 1
        }
        public enum ConditionalTargetRuleEnum
        {
            _ = 0,
            Stat = 1,
            AliveCount = 2
        }

        public enum ConditionalTargetCountEnum
        {
            _0 = 0,
            _1 = 1,
            _2 = 2,
            _3 = 3,
            _4 = 4,
            _5 = 5
        }

        public enum PercentageEnum
        {
            _0 = 0,
            _10 = 10,
            _20 = 20,
            _30 = 30,
            _40 = 40,
            _50 = 50,
            _60 = 60,
            _70 = 70,
            _80 = 80,
            _90 = 90,
            _100 = 100
        }
    }
}
