using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IItemService
    {
        Item Get(Guid id);
        IList<Item> All();
        Item Create(Item entity);
        Item Edit(Item entity);
        bool Remove(Guid id);

    }
}
