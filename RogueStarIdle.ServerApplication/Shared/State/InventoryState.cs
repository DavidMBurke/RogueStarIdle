using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;


namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class InventoryState
    {
        public List<Item> inventory { get; set; } = new List<Item>();
        private Dictionary<string, bool> expandedViews = new Dictionary<string, bool>();

        public async Task<IEnumerable<Item>> GetItemsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(inventory);
            }

            return inventory.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<IEnumerable<Item>> GetItemsByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return await Task.FromResult(inventory);
            }
            var taggedItems = inventory.Where(x => x.Tags.Any(t => t.Contains(tag, StringComparison.OrdinalIgnoreCase)));
            return taggedItems;
        }
    }


}
