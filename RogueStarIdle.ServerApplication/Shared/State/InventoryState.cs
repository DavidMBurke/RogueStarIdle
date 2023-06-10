using Microsoft.AspNetCore.Http.Features;
using RogueStarIdle.CoreBusiness;


namespace RogueStarIdle.ServerApplication.Shared.State
{
    public class InventoryState
    {
        public List<Item> Inventory { get; set; } = new List<Item>();
        public List<Item> ShipStorage { get; set; } = new List<Item>();
        public List<Item> AzuraliPlainsStorage { get; set; } = new List<Item>();
        public List<Item> NoxiousWetlandsStorage { get; set; } = new List<Item>();
        public List<Item> rockyOutcroppingStorage { get; set; } = new List<Item>();
        public List<Item> AncientRuinsStorage { get; set; } = new List<Item>();
        public List<Item> PlateauStorage { get; set; } = new List<Item>();
        public List<Item> CobaltForestStorage { get; set; } = new List<Item>();
        public List<Item> DarkChasmStorage { get; set; } = new List<Item>();
        public List<Item> AncientFactoryStorage { get; set; } = new List<Item>();
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
                return await Task.FromResult(Inventory);
            }
            return Inventory.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<IEnumerable<Item>> GetItemsByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return await Task.FromResult(Inventory);
            }
            var taggedItems = Inventory.Where(x => x.Tags.Any(t => t.Contains(tag, StringComparison.OrdinalIgnoreCase)));
            return taggedItems;
        }

        // When item equipped / unequipped, item is replaced with item with updated status. This is tradeoff d/t storing duplicate items as one item with different Quantity.

        public async void AddToInventory(List<Item> inventory, Item item, int quantityAdded = 1)
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
            if (matchingItem == null)
            {
                newItem.Quantity = quantityAdded;
                inventory.Add(newItem);
            } else
            {
                matchingItem.Quantity += quantityAdded;
            }
            await NotifyStateChanged();
        }

        public async void RemoveFromInventory(List<Item> inventory, Item item, int quantityRemoved = 1)
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

        public async void Transfer(List<Item> storageFrom, List<Item> storageTo)
        {
            foreach (Item item in storageFrom)
            {
                AddToInventory(storageTo, item, item.Quantity);
            }
            storageFrom.Clear();
        }
    }
}
