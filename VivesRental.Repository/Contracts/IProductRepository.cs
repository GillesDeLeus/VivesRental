using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Results;

namespace VivesRental.Repository.Contracts
{
    public interface IProductRepository
    {
	    IEnumerable<Product> GetAll(ProductIncludes includes = null);
        IEnumerable<ProductResult> GetAllResult(DateTime availableFromDateTime, DateTime availableUntilDateTime, ProductIncludes includes = null);


        IEnumerable<Product> Find(Func<Product, bool> predicate, ProductIncludes includes = null);

        IEnumerable<ProductResult> FindResult(Func<Product, bool> predicate, DateTime availableFromDateTime, DateTime availableUntilDateTime,
            ProductIncludes includes = null);
        Product Get(Guid id, ProductIncludes includes = null);

        void Add(Product product);

        void Remove(Guid id);
    }
}
