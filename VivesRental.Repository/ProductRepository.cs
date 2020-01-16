using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Repository
{
    public class ProductRepository :IProductRepository
    {
        private readonly IVivesRentalDbContext _context;

        public ProductRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> Find()
        {
            return _context.Products
                .AsEnumerable();
        }

        public IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate)
        {
            return _context.Products
                .Where(predicate)
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
