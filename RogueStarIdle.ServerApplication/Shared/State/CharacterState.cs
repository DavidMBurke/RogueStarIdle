using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;
using RogueStarIdle.ServerApplication.Shared.State;

namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class CharacterState {
        public Character MainCharacter { get; set; } = new Character()
        {
            Image = "/Images/Player.png"
        };
        public List<Character> Characters { get; set; } = new List<Character>();
    }
}
