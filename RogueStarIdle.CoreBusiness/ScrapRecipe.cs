using System.Diagnostics.CodeAnalysis;

namespace RogueStarIdle.CoreBusiness
{
    public class ScrapRecipe
    {
        public Item Item { get; set; } = new Item();
        public List<Scrap> ScrapList { get; set; } = new List<Scrap>();
    }
}
