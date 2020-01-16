using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface IProductRepository
    {
        IEnumerable<Product> Find();
	    IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate);

        void Add(Product product);

        void Remove(Guid id);
    }
}
