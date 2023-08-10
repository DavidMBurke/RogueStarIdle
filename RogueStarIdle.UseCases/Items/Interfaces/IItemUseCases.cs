using RogueStarIdle.CoreBusiness;

namespace RogueStarIdle.UseCases.Items.Interfaces
{
    public interface IItemUseCases
    {
        Task<Item> ExecuteAsync(int id);
        Task<Item> ExecuteAsync(string name);
    }
    public interface IViewItemsByNameUseCase
    {
        Task<IEnumerable<Item>> ExecuteAsync(string name = "");
    }
    public interface IViewItemsByTagUseCase
    {
        Task<IEnumerable<Item>> ExecuteAsync(string name = "");
    }
}