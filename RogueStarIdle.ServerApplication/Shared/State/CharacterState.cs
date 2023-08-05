using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CharacterState {
        public Character MainCharacter { get; set; } = new Character()
        {
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
        public List<Character> Characters { get; set; } = new List<Character>();
    }
}
