using RogueStarIdle.CoreBusiness;
using RogueStarIdle.UseCases.Items.Interfaces;
using RogueStarIdle.UseCases.Items.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueStarIdle.UseCases.Items
{
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
