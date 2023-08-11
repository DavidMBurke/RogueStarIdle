using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.PluginInterfaces;

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
                    CurrentHealth = 100,
                    Images = new MobImageUrls
                    {
                        Stationary = "/Images/Tardihop.png",
                        Attacking = "/Images/Tardihop_Attack.gif",
                        Dead = "/Images/Tardihop_Dead.png"
                    },
                    Loot = new List<ItemDrop>() 
                    {
                        new ItemDrop (itemsRepository.items[15], 1, 1, 1, 2),
                        new ItemDrop (itemsRepository.items[2], 1, 1, 1, 1),
                        new ItemDrop (itemsRepository.items[13], 1, 1, 1, 1)
                    }
                }
            };

        }
        public async Task<Mob> GetMobByIdAsync(int id)
        {
            return mobs.First(i => i.Id == id);
        }
    }
}