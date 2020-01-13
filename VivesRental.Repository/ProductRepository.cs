using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Mappers;
using VivesRental.Repository.Results;

namespace VivesRental.Repository
{
    public class ProductRepository :IProductRepository
    {
        private readonly IVivesRentalDbContext _context;

        public ProductRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Product Get(Guid id, ProductIncludes includes = null)
	    {
            return _context.Products
                .AddIncludes(includes)
                .FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Product> GetAll(ProductIncludes includes = null)
		{
            return _context.Products
                .AddIncludes(includes)
                .AsEnumerable();
        }

        public IEnumerable<ProductResult> GetAllResult(ProductIncludes includes = null)
        {
            return _context.Products
                .AddIncludes(includes)
                .MapToResults()
                .AsEnumerable();
        }

        public IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate, ProductIncludes includes = null)
		{
            return _context.Products
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public IEnumerable<ProductResult> FindResult(Expression<Func<Product, bool>> predicate, ProductIncludes includes = null)
        {
            return _context.Products
                .AddIncludes(includes)
                .Where(predicate)
                .MapToResults()
                .AsEnumerable();
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Remove(Guid id)
        {
            var localEntity = _context.Products.Local.SingleOrDefault(e => e.Id == id);
            if (localEntity == null)
            {
                var entity = new Product { Id = id };
                _context.Products.Attach(entity);
                _context.Products.Remove(entity);
            }
            else
            {
                _context.Products.Remove(localEntity);
            }
        }
    }
}
