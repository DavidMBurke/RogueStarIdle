using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.Interfaces;
using RogueStarIdle.UseCases.Items.PluginInterfaces;


namespace RogueStarIdle.UseCases.Items
{
    public class ItemUseCases : IItemUseCases
    {
        private readonly IItemsRepository itemsRepository;

        public ItemUseCases(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }
        public async Task<Item> ExecuteAsync(int id)
        {
            return await itemsRepository.GetItemByIdAsync(id);
        }

        public async Task<Item> ExecuteAsync(string name)
        {
            return await itemsRepository.GetItemByNameAsync(name);
        }
    }
    public class ViewItemsByNameUseCase : IViewItemsByNameUseCase
    {
        private readonly IItemsRepository itemsRepository;

        public ViewItemsByNameUseCase(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }
        public async Task<IEnumerable<Item>> ExecuteAsync(string name = "")
        {
            return await itemsRepository.GetItemsByNameAsync(name);
        }
    }
    public class ViewItemsByTagUseCase : IViewItemsByTagUseCase
    {
        private readonly IItemsRepository itemsRepository;

        public ViewItemsByTagUseCase(IItemsRepository itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }
        public async Task<IEnumerable<Item>> ExecuteAsync(string tag = "")
        {
            var items = await itemsRepository.GetItemsByTagAsync(tag);
            return items;
        }
    }
}
