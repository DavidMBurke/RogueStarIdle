using RogueStarIdle.CoreBusiness;

namespace RogueStarIdle.UseCases.Items.Interfaces
{
    public interface IGetItemByIdUseCase
    {
        Task<Item> ExecuteAsync(int id);
    }
}