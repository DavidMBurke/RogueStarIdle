using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.Interfaces;
using RogueStarIdle.UseCases.Items.PluginInterfaces;


namespace RogueStarIdle.UseCases.Mobs
{
    public class MobUseCases : IMobUseCases
    {
        private readonly IMobsRepository mobsRepository;

        public MobUseCases(IMobsRepository mobsRepository)
        {
            this.mobsRepository = mobsRepository;
        }
        public async Task<Mob> ExecuteAsync(int id)
        {
            return await mobsRepository.GetMobByIdAsync(id);
        }
    }
}
