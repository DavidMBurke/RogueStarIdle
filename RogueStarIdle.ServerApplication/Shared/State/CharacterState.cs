﻿using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CharacterState {
        public Character SelectedCharacter { get; set; } = new Character();
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

        public List<Character> Party { get; set; } = new List<Character>();

        public List<Character> SideCharacters { get; set; } = new List<Character>()
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
            },
            new Character()
            {
                Id = 2,
                Name = "Side Character 2",
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
            },
            new Character()
            {
                Id = 3,
                Name = "Side Character 3",
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
            },
            new Character()
            {
                Id = 4,
                Name = "Side Character 4",
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
        public async void AddCombatRule(Character character)
        {
            character.CombatRules.Add(new CombatRule());
            await NotifyStateChanged();
        }
        public async void RemoveCombatRule(Character character)
        {
            if (character.CombatRules.Count <= 1)
            {
                return;
            }
            character.CombatRules.RemoveAt(character.CombatRules.Count - 1);
            await NotifyStateChanged();
        }

        public async void SelectCharacter(Character character)
        {
            SelectedCharacter = character;
            await NotifyStateChanged();
        }
    }
}
