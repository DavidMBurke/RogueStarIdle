using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.PluginInterfaces;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Xml.Linq;
using static RogueStarIdle.CoreBusiness.EquipmentSet;

namespace RogueStarIdle.PlugIns.InMemory
{
    public class ItemsRepository : IItemsRepository
    {
        public List<Item> items;

        public ItemsRepository()
        {
            items = new List<Item>()
            {
                new Item {
                    Id = 0,
                    Name = "Animal Parts",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Component", "Scrap"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/AnimalParts.png"
                },
                new Item {
                    Id = 1,
                    Name = "Animal Skins",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Component", "Scrap"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/AnimalSkins.png"
                },
                new Item {
                    Id = 2,
                    Name = "Small Bones",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Tardihop", "Component"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/SmallBones.png"
                },
                new Item {
                    Id = 3,
                    Name = "Azurali Grass",
                    BuyPrice = 10,
                    SellPrice = 50,
                    Tags = new List<string>{"Plant", "Forageable"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/AzuraliGrass.png"
                },
                new Item {
                    Id = 4,
                    Name = "Bone Knife",
                    BuyPrice = 10,
                    SellPrice = 50,
                    IsWeapon= true,
                    AttackSpeed = 100,
                    Tags = new List<string>{"Weapon", "Knife", "One-Handed" },
                    IsEquippable = true,
                    IsMelee = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.LeftWeapon,(int)EquipmentSlotEnum.RightWeapon},
                    MeleeToHit = 5,
                    MinBaseDamage = 10,
                    MaxBaseDamage = 20,
                    PercentSlashingDamage = 100,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "/Images/Thumbnails/BoneKnife.png",
                    Images = new ImageUrls
                        {
                            Stationary = "/Images/Player/Sword.png",
                            Attacking2HandMelee = "/Images/Player/Attack_Sword.gif"
                        }
                },
                new Item {
                    Id = 5,
                    Name = "Tardihop Fur Hat",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Head},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "/Images/Thumbnails/TardihopFurHat.png"
                },
                new Item {
                    Id = 6,
                    Name = "Tardihop Fur Gloves",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Hands"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Hands},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "/Images/Thumbnails/TardihopFurGloves.png"
                },
                new Item {
                    Id = 7,
                    Name = "Tardihop Fur Shirt",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Torso"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Torso},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "/Images/Thumbnails/TardihopFurShirt.png",
                    Images= new ImageUrls
                        {
                            Stationary = "/Images/Player/Shirt_Pink.png",
                            Attacking2HandMelee = "/Images/Player/Attack_Shirt_Pink.gif"
                        }
                },
                new Item {
                    Id = 8,
                    Name = "Tardihop Fur Pants",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Legs"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Legs},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "Images/Thumbnails/TardihopFurPants.png"
                },
                new Item {
                    Id = 9,
                    Name = "Tardihop Fur Boots",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Feet"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Feet},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "Images/Thumbnails/TardihopFurBoots.png"
                },
                new Item {
                    Id = 10,
                    Name = "Carapig Chitin Mask",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Face"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Head},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "Images/Thumbnails/CarapigChitinMask.png"
                },
                new Item {
                    Id = 11,
                    Name = "Carapig Chitin Armor Top",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Torso"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Torso},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "Images/Thumbnails/CarapigChitinArmorTop.png"
                },
                new Item {
                    Id = 12,
                    Name = "Carapig Chitin Armor Legs",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Legs"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Legs},
                    IsArmor = true,
                    MeleeDefense = 10,
                    EnergyDefense = 2,
                    EnergyDamageReduction = 1,
                    ShockDamageReduction = 5,
                    QualityLevel = 1,
                    MaxQualityLevel = 15,
                    Thumbnail = "Images/Thumbnails/CarapigChitinArmorLegs.png"
                },
                new Item {
                    Id = 13,
                    Name = "Tardihop Guts",
                    BuyPrice = 10,
                    SellPrice = 5,
                    Tags = new List<string>{"Tardihop", "Component"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/TardihopGuts.png"
                },
                new Item {
                    Id = 14,
                    Name = "Stick",
                    BuyPrice = 2,
                    SellPrice = 0,
                    Tags = new List<string>{"Plant", "Forageable"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/Stick.png"
                },
                new Item {
                    Id = 15,
                    Name = "Tardihop Fur",
                    BuyPrice = 10,
                    SellPrice = 2,
                    Tags = new List<string>{"Tardihop", "Component"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/TardihopFur.png"
                },
                new Item {
                    Id = 16,
                    Name = "Plant Fiber",
                    BuyPrice = 1,
                    SellPrice = 0,
                    Tags = new List<string>{"Component", "Scrap"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/PlantFiber.png"
                },  
                new Item {
                    Id = 17,
                    Name = "Tardihop Bait",
                    BuyPrice = 1,
                    SellPrice = 0,
                    Tags = new List<string>{"Survival", "Bait"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/TardihopBait.png"
                },
                new Item {
                    Id = 18,
                    Name = "Cloth Bandage",
                    Consumable = true,
                    HealthRestored = 5,
                    StacksInEquipmentSlot = true,
                    BuyPrice = 1,
                    SellPrice = 1,
                    IsEquippable = true,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Aid},
                    Tags = new List<string>{"Aid"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/ClothBandage.png"
                },
                new Item {
                    Id = 19,
                    Name = "Bonemeal",
                    BuyPrice = 1,
                    SellPrice = 1,
                    Tags = new List<string>{"Farming"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/Bonemeal.png"
                },
                new Item {
                    Id = 20,
                    Name = "Rope",
                    BuyPrice = 1,
                    SellPrice = 1,
                    Tags = new List<string>{"Construction"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/Rope.png"
                },
                new Item {
                    Id = 21,
                    Name = "Cloth Shoes",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Feet"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Feet},
                    IsArmor = true,
                    MeleeDefense = 1,
                    QualityLevel = 1,
                    MaxQualityLevel = 10,
                    Thumbnail = "Images/Thumbnails/ClothShoes.png"
                },
                new Item {
                    Id = 22,
                    Name = "Cloth Hat",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Head"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Head},
                    IsArmor = true,
                    MeleeDefense = 1,
                    QualityLevel = 1,
                    MaxQualityLevel = 10,
                    Thumbnail = "Images/Thumbnails/ClothHat.png"
                },
                new Item {
                    Id = 23,
                    Name = "Cloth Shirt",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Torso"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Torso},
                    IsArmor = true,
                    MeleeDefense = 1,
                    QualityLevel = 1,
                    MaxQualityLevel = 10,
                    Thumbnail = "Images/Thumbnails/ClothShirt.png"
                },
                new Item {
                    Id = 24,
                    Name = "Cloth Pants",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Legs"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Legs},
                    IsArmor = true,
                    MeleeDefense = 1,
                    QualityLevel = 1,
                    MaxQualityLevel = 10,
                    Thumbnail = "Images/Thumbnails/ClothPants.png"
                },
                new Item {
                    Id = 25,
                    Name = "Cloth Gloves",
                    BuyPrice = 10,
                    SellPrice = 10,
                    Tags = new List<string> {"Armor", "Hands"},
                    IsEquippable = true,
                    Quantity = 1,
                    EquipmentSlots = new List<int>{(int)EquipmentSlotEnum.Hands},
                    IsArmor = true,
                    MeleeDefense = 1,
                    QualityLevel = 1,
                    MaxQualityLevel = 10,
                    Thumbnail = "Images/Thumbnails/ClothGloves.png"
                },
                new Item {
                    Id = 26,
                    Name = "Stone",
                    BuyPrice = 1,
                    SellPrice = 1,
                    Tags = new List<string>{"Construction", "Component"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/Stone.png"
                },        
                new Item {
                    Id = 27,
                    Name = "Purple Berries",
                    BuyPrice = 1,
                    SellPrice = 1,
                    Tags = new List<string>{"Aid"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/PurpleBerries.png"
                },
                new Item {
                    Id = 28,
                    Name = "Purple Berry Seeds",
                    BuyPrice = 1,
                    SellPrice = 1,
                    Tags = new List<string>{"Aid"},
                    Quantity = 1,
                    Thumbnail = "/Images/Thumbnails/PurpleBerrySeeds.png"
                },
                new Item {
                    Id = 29,
                    Name = "Carapig Chitin",
                    BuyPrice = 10,
                    SellPrice = 2,
                    Tags = new List<string>{"Carapig", "Component"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/CarapigChitin.png"
                },
                new Item {
                    Id = 30,
                    Name = "Carapig Scent Gland",
                    BuyPrice = 10,
                    SellPrice = 2,
                    Tags = new List<string>{"Carapig", "Component"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/CarapigScentGland.png"
                },
                new Item {
                    Id = 31,
                    Name = "Carapig Bait",
                    BuyPrice = 10,
                    SellPrice = 2,
                    Tags = new List<string>{"Carapig", "Bait"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/CarapigBait.png"
                },                new Item {
                    Id = 32,
                    Name = "Grizzix Bait",
                    BuyPrice = 10,
                    SellPrice = 2,
                    Tags = new List<string>{"Grizzix", "Bait"},
                    Quantity = 1,
                    Thumbnail = "Images/Thumbnails/GrizzixBait.png"
                },
            };
        }

        public enum ItemsEnum
        {
            AnimalParts = 0,
            AnimalSkins = 1,
            SmallBones = 2,
            AzuraliGrass = 3,
            BoneKnife = 4,
            TardihopFurHat = 5,
            TardihopFurGloves = 6,
            TardihopFurShirt = 7,
            TardihopFurPants = 8,
            TardihopFurBoots = 9,
            CarapigChitinMask = 10,
            CarapigChitinArmorTop = 11,
            CarapigChitinArmorLegs = 12,
            TardihopGuts = 13,
            Stick = 14,
            TardihopFur = 15,
            PlantFiber = 16,
            TardihopBait = 17,
            ClothBandage = 18,
            Bonemeal = 19,
            Rope = 20,
            ClothShoes = 21,
            ClothHat = 22,
            ClothShirt = 23,
            ClothPants = 24,
            ClothGloves = 25,
            Stone = 26,
            PurpleBerries = 27,
            PurpleBerrySeeds = 28,
            CarapigChitin= 29,
            CarapigScentGland = 30,
            CarapigBait = 31,
            GrizzixBait = 32
        }


        public async Task<IEnumerable<Item>> GetItemsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await Task.FromResult(items);
            }

            return items.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
        public async Task<IEnumerable<Item>> GetItemsByTagAsync(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return await Task.FromResult(items);
            }
            var taggedItems = items.Where(x => x.Tags.Any(t => t.Contains(tag, StringComparison.OrdinalIgnoreCase)));
            return taggedItems;
        }
        public async Task<Item> GetItemByIdAsync(int id)
        {
            return items.First(i => i.Id == id);
        }
        public async Task<Item> GetItemByNameAsync(string name)
        {
            return items.First(i => i.Name == name);
        }
    }
}