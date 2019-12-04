﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IProductRepository
    {
	    IEnumerable<Product> GetAll(ProductIncludes includes = null);

		IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate, ProductIncludes includes = null);

        Product Get(Guid id, ProductIncludes includes = null);

        void Add(Product product);

        void Remove(Guid id);
    }
}
