using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.PluginInterfaces;
using static RogueStarIdle.PlugIns.InMemory.ItemsRepository;

namespace RogueStarIdle.PlugIns.InMemory
{
    public class MobsRepository : IMobsRepository
    {
        public List<Mob> mobs;
        public ItemsRepository itemsRepository = new ItemsRepository();

        public MobsRepository()
        {
            mobs = new List<Mob>()
            {
                new Mob {
                    Id = 0,
                    Name = "Tardihop",
                    Stats = new Stats()
                    {
                        MaxHealth = 50,
                        SlashingDamageMin = 1,
                        SlashingDamageMax = 5,
                        AttackSpeed = 75 // 1.5 sec
                    },
                    CurrentHealth = 50,
                    Images = new MobImageUrls
                    {
                        Stationary = "/Images/Tardihop.png",
                        Attacking = "/Images/Tardihop_Attack.gif",
                        Dead = "/Images/Tardihop_Dead.png"
                    },
                    Loot = new List<ItemDrop>() 
                    {
                        new ItemDrop (itemsRepository.items[(int)ItemsEnum.TardihopFur], 1, 1, 1, 2),
                        new ItemDrop (itemsRepository.items[(int)ItemsEnum.SmallBones], 1, 1, 1, 1),
                        new ItemDrop (itemsRepository.items[(int)ItemsEnum.TardihopGuts], 1, 1, 1, 1)
                    }
                },
                new Mob
                {
                    Id = 1,
                    Name = "Carapig",
                    Stats = new Stats()
                    {
                        MaxHealth = 200,
                        CrushingDamageMin = 5,
                        CrushingDamageMax = 10,
                        AttackSpeed = 75 // 1.5 sec
                    },
                    CurrentHealth = 200,
                    Images = new MobImageUrls
                    {
                        Stationary = "/Images/Carapig.png",
                        Attacking = "/Images/Carapig_Attack.png",
                        Dead = "/Images/Carapig_Dead.png"
                    },
                    Loot = new List<ItemDrop>()
                    {
                        new ItemDrop (itemsRepository.items[(int)ItemsEnum.CarapigChitin], 1, 1, 1, 3),
                        new ItemDrop (itemsRepository.items[(int)ItemsEnum.CarapigScentGland], 1, 4, 1, 1)
                    }
                }
            };

        }

        public enum MobsEnum
        {
            Tardihop = 0
        }
        public async Task<Mob> GetMobByIdAsync(int id)
        {
            return mobs.First(i => i.Id == id);
        }
    }
}