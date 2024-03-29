﻿using RogueStarIdle.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueStarIdle.UseCases.Items.PluginInterfaces
{
    public interface IItemsRepository
    {
        Task<IEnumerable<Item>> GetItemsByNameAsync(string name);
        Task<IEnumerable<Item>> GetItemsByTagAsync(string tag);
        Task<Item> GetItemByIdAsync(int id);

        Task<Item> GetItemByNameAsync(string name);
    }
}
