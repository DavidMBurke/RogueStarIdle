namespace RogueStarIdle.CoreBusiness
{
    public class Stats
    {
        public bool IsUsingMelee { get; set; } = false;
        public bool IsUsingRanged { get; set; } = false;
        public bool IsUsingPsychic { get; set; } = false;
        public bool IsUsingExplosive { get; set; } = false;
        public int MeleeToHit { get; set; } = 0;
        public int RangedToHit { get; set; } = 0;
        public int PsychicToHit { get; set; } = 0;
        public int ExplosiveToHit { get; set; } = 0;
        public int AttackSpeed { get; set; } = 0; //Ticks between attacks, lower is faster (1 tick = 20 ms)
        public int CurrentHealth { get; set; } = 0;
        public int MaxHealth { get; set; } = 0;
        public int EnergyDefense { get; set; } = 0;
        public int KineticDefense { get; set; } = 0;
        public int PsychicDefense { get; set; } = 0;
        public int MeleeDefense { get; set; } = 0;
        public int RangedDefense { get; set; } = 0;
        public int ExplosiveDefense { get; set; } = 0;
        public int EnergyDR { get; set; } = 0;
        public int FireDR { get; set; } = 0;
        public int AcidDR { get; set; } = 0;
        public int PoisonDR { get; set; } = 0;
        public int ShockDR { get; set; } = 0;
        public int KineticDR { get; set; } = 0;
        public int PiercingDR { get; set; } = 0;
        public int CrushingDR { get; set; } = 0;
        public int SlashingDR { get; set; } = 0;
        public int PsychicDR { get; set; } = 0;
        public int FireDamageMin { get; set; } = 0;
        public int FireDamageMax { get; set; } = 0;
        public int AcidDamageMin { get; set; } = 0;
        public int AcidDamageMax { get; set; } = 0;
        public int PoisonDamageMin { get; set; } = 0;
        public int PoisonDamageMax { get; set; } = 0;
        public int ShockDamageMin { get; set; } = 0;
        public int ShockDamageMax { get; set; } = 0;
        public int PiercingDamageMin { get; set; } = 0;
        public int PiercingDamageMax { get; set; } = 0;
        public int CrushingDamageMin { get; set; } = 0;
        public int CrushingDamageMax { get; set; } = 0;
        public int SlashingDamageMin { get; set; } = 0;
        public int SlashingDamageMax { get; set; } = 0;
    }
}