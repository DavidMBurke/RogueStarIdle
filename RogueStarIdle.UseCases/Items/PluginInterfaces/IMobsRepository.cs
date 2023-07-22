using RogueStarIdle.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueStarIdle.UseCases.Items.PluginInterfaces
{
    public interface IMobsRepository
    {
        Task<Mob> GetMobByIdAsync(int id);
    }
}
