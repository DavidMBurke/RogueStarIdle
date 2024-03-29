﻿namespace RogueStarIdle.CoreBusiness
{
    public class Mob
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Stats Stats { get; set; } = new Stats();
        public List<ItemDrop> Loot { get; set; } = new List<ItemDrop>();
        public int CurrentHealth { get; set; } = 0;
        public bool IsAlive { get; set; } = true;
        public MobImageUrls Images { get; set; } = new MobImageUrls();
        public bool TriggerAttackAnimation { get; set; } = false;

        public Mob Clone()
        {
            return new Mob()
            {
                Id = Id,
                Name = Name,
                Stats = Stats.Clone(),
                Loot = new List<ItemDrop>(Loot),
                CurrentHealth = CurrentHealth,
                IsAlive = IsAlive,
                Images = Images,
            };
        }
        public void Attack(Character defender)
        {
            if (defender == null)
            {
                return;
            }
            TriggerAttackAnimation = true;
            Random rand = new Random();
            int hitRoll = rand.Next(20);
            int blockRoll = defender.Equipment.Stats.MeleeDefense + defender.BlockSkill.Level + rand.Next(20);
            int damage = CalculateTotalDamage(Stats, defender.Equipment.Stats);
            if (hitRoll > blockRoll)
            {
                defender.CurrentHealth -= damage;
                if (defender.CurrentHealth < 0) { 
                    defender.CurrentHealth = 0;
                }
                defender.VitalitySkill.addXp(1);
            }
            else
            {
                defender.BlockSkill.addXp(1);
            }
        }

        public int CalculateTotalDamage(Stats attacker, Stats defender)
        {
            Random rand = new Random();
            int damage = 0;
            damage += CalculateDamageByType(attacker.PiercingDamageMin, attacker.PiercingDamageMax, defender.PiercingDR);
            damage += CalculateDamageByType(attacker.SlashingDamageMin, attacker.SlashingDamageMax, defender.SlashingDR);
            damage += CalculateDamageByType(attacker.CrushingDamageMin, attacker.CrushingDamageMax, defender.CrushingDR);
            damage += CalculateDamageByType(attacker.AcidDamageMin, attacker.AcidDamageMax, defender.AcidDR);
            damage += CalculateDamageByType(attacker.FireDamageMin, attacker.FireDamageMax, defender.FireDR);
            damage += CalculateDamageByType(attacker.ShockDamageMin, attacker.ShockDamageMax, defender.ShockDR);
            damage += CalculateDamageByType(attacker.PoisonDamageMin, attacker.PoisonDamageMax, defender.PoisonDR);
            return damage;
        }

        public int CalculateDamageByType(int min, int max, int dr)
        {
            Random rand = new Random();
            int baseDamage = min + rand.Next(1 + max - min);
            int reducedDamage = (baseDamage * (100 - dr)) / 100;
            return reducedDamage;
        }

        public void Die()
        {
            IsAlive = false;
        }

        public void Revive()
        { 
            IsAlive = true; 
        }
    }
}