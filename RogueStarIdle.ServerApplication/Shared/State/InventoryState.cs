using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;


namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class InventoryState
    {
        public List<Item> inventory { get; set; } = new List<Item>();
        private Dictionary<string, bool> expandedViews = new Dictionary<string, bool>();
        public event Func<Task> OnChange;
        private async Task NotifyStateChanged()
        {
            if (OnChange == null)
                return;
            await OnChange.Invoke();
        }
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

        // When item equipped / unequipped, item is replaced with item with updated status. This is tradeoff d/t storing duplicate items as one item with different Quantity.

        public async void addToInventory(Item item, int quantityAdded = 1)
        {
            if (item == null)
            {
                return;
            }
            if (item.StacksInEquipmentSlot)
            {
                quantityAdded = item.Quantity;
            }
            Item matchingItem = inventory.FirstOrDefault(i => (i.Id == item.Id && i.QualityLevel == item.QualityLevel && i.Equipped == item.Equipped), null); 
            Item newItem = item.createCopy();
            newItem.Quantity = quantityAdded;
            inventory.Add(newItem);
            await NotifyStateChanged();
        }

        public async void removeFromInventory(Item item, int quantityRemoved = 1)
        {
            Item itemInInventory = inventory.FirstOrDefault(i => (i.Id == item.Id && i.QualityLevel == item.QualityLevel && i.Equipped == item.Equipped), null);
            if (itemInInventory == null)
            {
                return;
            }
            itemInInventory.Quantity -= quantityRemoved;
            if (itemInInventory.Quantity <= 0)
            {
                inventory.Remove(itemInInventory);
            }
            await NotifyStateChanged();
        }
    }
}
