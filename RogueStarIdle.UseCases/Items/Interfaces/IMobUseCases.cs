using RogueStarIdle.CoreBusiness;

namespace RogueStarIdle.UseCases.Items.Interfaces
{
    public interface IMobUseCases
    {
        Task<Mob> ExecuteAsync(int id);
    }
}