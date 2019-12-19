using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Product Get(Guid id);
        Product Get(Guid id, ProductIncludes includes);
        IList<Product> All();
        IList<Product> All(ProductIncludes includes);
        Product Create(Product entity);
        Product Edit(Product entity);
        bool Remove(Guid id);

    }
}
