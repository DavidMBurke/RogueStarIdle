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
}
