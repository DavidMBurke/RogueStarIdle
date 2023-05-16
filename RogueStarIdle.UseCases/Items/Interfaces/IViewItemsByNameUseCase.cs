using RogueStarIdle.CoreBusiness;

namespace RogueStarIdle.UseCases.Items.Interfaces
{
    public interface IViewItemsByNameUseCase
    {
        Task<IEnumerable<Item>> ExecuteAsync(string name = "");
    }
}