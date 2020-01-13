using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.Repository.Contracts
{
    public interface IProductRepository
    {
	    IEnumerable<Product> GetAll();
        IEnumerable<ProductResult> GetAllResult(DateTime availableFromDateTime, DateTime availableUntilDateTime);
        
        IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate);

        IEnumerable<ProductResult> FindResult(DateTime availableFromDateTime, DateTime? availableUntilDateTime);

        Product Get(Guid id);

        void Add(Product product);

        void Remove(Guid id);
    }
}
