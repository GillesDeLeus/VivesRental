using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IItemRepository
    {
	    IEnumerable<Item> GetAll(ItemIncludes includes = null);

		IEnumerable<Item> Find(Expression<Func<Item, bool>> predicate, ItemIncludes includes = null);

        Item Get(Guid id, ItemIncludes includes = null);

        void Add(Item item);

        void Remove(Guid id);
    }
}
