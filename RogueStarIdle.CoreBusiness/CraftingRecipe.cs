using System.Diagnostics.CodeAnalysis;

namespace RogueStarIdle.CoreBusiness
{
    public class CraftingRecipe
    {
        public Item Item { get; set; } = new Item();
        public List<(Item, int)> Ingredients { get; set; } = new List<(Item, int)>();
        public List<(string, int)> RequiredSkillLevels { get; set; } = new List<(string, int)>();
        public List<(string, int)> XpReward { get; set; } = new List<(string, int)>();
        public bool Unlocked { get; set; } = false;
    }
}
