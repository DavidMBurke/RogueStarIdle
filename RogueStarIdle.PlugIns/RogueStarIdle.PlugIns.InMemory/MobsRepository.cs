using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.PluginInterfaces;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace RogueStarIdle.PlugIns.InMemory
{
    public class MobsRepository : IMobsRepository
    {
        public List<Mob> _mobs;

        public MobsRepository()
        {
            _mobs = new List<Mob>()
            {
                new Mob {
                    Id = 1,
                    Name = "Tardihop",
                    Stats = new Stats()
                    {
                        CurrentHealth = 100,
                        MaxHealth = 100,
                        SlashingDamageMin = 0,
                        SlashingDamageMax = 10
                    }
                }
            };

        }
        public async Task<Mob> GetMobByIdAsync(int id)
        {
            return _mobs.First(i => i.Id == id);
        }
    }
}