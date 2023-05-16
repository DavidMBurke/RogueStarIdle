﻿using RogueStarIdle.CoreBusiness;

namespace RogueStarIdle.UseCases.Items.Interfaces
{
    public interface IViewItemsByTagUseCase
    {
        Task<IEnumerable<Item>> ExecuteAsync(string name = "");
    }
}