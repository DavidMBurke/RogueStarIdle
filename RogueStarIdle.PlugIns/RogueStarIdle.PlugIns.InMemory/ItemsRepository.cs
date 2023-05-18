using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.PluginInterfaces;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace RogueStarIdle.PlugIns.InMemory
{
    public class ItemsRepository : IItemsRepository
    {
        private List<Item> _items;

        public ItemsRepository()
        {
            _items = new List<Item>()
            {
                new Item {
                    Id = 1,
                    Name = "Animal Parts",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Component", "Scrap"},
                    Quantity = 1 },
                new Item {
                    Id = 2,
                    Name = "Animal Skins",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Component", "Scrap"},
                    Quantity = 1 },
                new Item {
                    Id = 3,
                    Name = "Azurali Grass",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Plant", "Forageable"},
                    Quantity = 1 },
                new Item {
                    Id = 4,
                    Name = "Bone Knife",
                    BuyPrice = 10,
                    SellPrice = 50,
                    IsWeapon= true,
                    Tags = new List<string>{"Weapon", "Knife", "One-Handed" },
                    IsEquippable = true,
                    IsMelee = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{9,13},
                    ToHitModifier = 10,
                    MinBaseDamage = 10,
                    MaxBaseDamage = 20,
                    PercentSlashingDamage = 100,
                    QualityLevel = 4,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 5,
                    Name = "Tardihop Fur Hat",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{1},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 6,
                    Name = "Tardihop Fur Gloves",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Hands"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{8},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 7,
                    Name = "Tardihop Fur Shirt",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{3},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 8,
                    Name = "Tardihop Fur Pants",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{5},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 9,
                    Name = "Tardihop Fur Boots",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{6},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 10,
                    Name = "Carapig Chitin Mask",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{1},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 11,
                    Name = "Carapig Chitin Armor Top",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Chest"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{3},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15},
                new Item {
                    Id = 12,
                    Name = "Carapig Chitin Armor Legs",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Legs"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{5},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 2,
                    MaxQualityLevel = 15}
            };
        }
        public async Task<IEnumerable<Item>> GetItemsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(_items);
            }

            return _items.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<IEnumerable<Item>> GetItemsByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return await Task.FromResult(_items);
            }
            var taggedItems = _items.Where(x => x.Tags.Any(t => t.Contains(tag, StringComparison.OrdinalIgnoreCase)));
            return taggedItems;
        }
    }
}