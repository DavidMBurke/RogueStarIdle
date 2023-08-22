using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CharacterState {
        public Character MainCharacter { get; set; } = new Character()
        {
            Id = 0,
            Name = "Main Character",
            Images = new CharacterImageUrls()
            {
                Head = new ImageUrls()
                {
                    Stationary = "/Images/Player/Head.png",
                    Attacking2HandMelee = "/Images/Player/Attack_head.gif"
                },
                Torso = new ImageUrls()
                {
                    Stationary = "/Images/Player/Shirt.png",
                    Attacking2HandMelee = "/Images/Player/Attack_Shirt.gif"
                },
                Legs = new ImageUrls()
                {
                    Stationary = "/Images/Player/Pants.png",
                    Attacking2HandMelee = "/Images/Player/Attack_Pants.gif"
                },
                Feet = new ImageUrls()
                {
                    Stationary = "/Images/Player/Shoes.png",
                    Attacking2HandMelee = "/Images/Player/Attack_Shoes.gif"
                }
            }
        };
        public List<Character> Characters { get; set; } = new List<Character>()
        {
            new Character()
            {
                Id = 1,
                Name = "Side Character 1",
                Images = new CharacterImageUrls()
                {
                    Head = new ImageUrls()
                    {
                        Stationary = "/Images/Player/Head.png",
                        Attacking2HandMelee = "/Images/Player/Attack_head.gif"
                    },
                    Torso = new ImageUrls()
                    {
                        Stationary = "/Images/Player/Shirt.png",
                        Attacking2HandMelee = "/Images/Player/Attack_Shirt.gif"
                    },
                    Legs = new ImageUrls()
                    {
                        Stationary = "/Images/Player/Pants.png",
                        Attacking2HandMelee = "/Images/Player/Attack_Pants.gif"
                    },
                    Feet = new ImageUrls()
                    {
                        Stationary = "/Images/Player/Shoes.png",
                        Attacking2HandMelee = "/Images/Player/Attack_Shoes.gif"
                    }
                }
            }
        };
        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }
        public void AddCombatRule(Character character)
        {
            character.CombatRules.Add(new CombatRule());
            NotifyStateChanged();
        }
        public void RemoveCombatRule(Character character)
        {
            if (character.CombatRules.Count <= 1)
            {
                return;
            }
            character.CombatRules.RemoveAt(character.CombatRules.Count - 1);
            NotifyStateChanged();
        }
    }
}
