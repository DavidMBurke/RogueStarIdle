﻿using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class ScavengingState {
        public bool IsScavenging { get; set; } = false;
        public int TicksUntilScavengeAttempt { get; set; } = 0;
        public int TicksBetweenScavengeAttempts { get; set; } = 50;
        public List<(Item item, int dropChanceNum, int dropChanceDenom, int quantityRangeMin, int quantityRangeMax)>? ScavengeableItems { get; set; } = null;
        public List<Item>? ScavengedItems { get; set; } = null;
        public InventoryState? inventoryState;
        public CharacterState? characterState;
        public int SurvivalXpAtLocation { get; set; } = 0;
        public ScavengingState(InventoryState inventoryState, CharacterState characterState)
        {
            this.inventoryState = inventoryState;
            this.characterState = characterState;
        }
        public void ScavengeTicks ()
        {   
            if (!IsScavenging)
            {
                return;
            }
            if (TicksUntilScavengeAttempt > 0)
            {
                TicksUntilScavengeAttempt--;
                return;
            }
            Scavenge();
            TicksUntilScavengeAttempt = TicksBetweenScavengeAttempts;
        }

        public void Scavenge ()
        {
            if (ScavengeableItems == null)
            {
                return;
            }
            List<Item> foundItems = new List<Item>();
            Random rand = new Random ();
            foreach (var item in ScavengeableItems)
            {
                int roll = rand.Next(item.dropChanceDenom) + 1;
                if (item.dropChanceNum >= roll)
                {
                    continue;
                }
                int qty = rand.Next(item.quantityRangeMax-item.quantityRangeMin) + item.quantityRangeMin;
                item.item.Quantity = qty;
                foundItems.Add(item.item);
            }
            foreach (var item in foundItems)
            {
                inventoryState.addToInventory(item, item.Quantity);
            }
            characterState.mainCharacter.SurvivalSkill.Xp += SurvivalXpAtLocation;
            characterState.mainCharacter.SurvivalSkill.UpdateLevel();
            return;
        }
    }
}