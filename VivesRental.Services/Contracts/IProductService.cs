using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Product Get(Guid id);
        IList<Product> All();
        Product Create(Product entity);
        Product Edit(Product entity);
        bool Remove(Guid id);

    }
}
