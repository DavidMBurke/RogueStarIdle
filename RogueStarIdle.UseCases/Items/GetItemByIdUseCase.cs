using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.Interfaces;
using RogueStarIdle.UseCases.Items.PluginInterfaces;


namespace RogueStarIdle.UseCases.Items
{
    public class GetItemByIdUseCase : IGetItemByIdUseCase
    {
        private readonly IItemsRepository itemsRepository;

        public GetItemByIdUseCase(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }
        public async Task<Item> ExecuteAsync(int id)
        {
            return await itemsRepository.GetItemByIdAsync(id);
        }
    }
}
