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

        public Product Get(Guid id)
        {
            return _context.Products
                .FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .AsEnumerable();
        }

        public IEnumerable<ProductResult> GetAllResult()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTIme = DateTime.MaxValue;
            return GetAllResult(fromDateTime, untilDateTIme);
        }

        public IEnumerable<ProductResult> GetAllResult(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Products
                .MapToResults(fromDateTime, untilDateTime)
                .AsEnumerable();
        }

        public IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate)
        {
            return _context.Products
                .Where(predicate)
                .AsEnumerable();
        }

        public IEnumerable<ProductResult> FindResult(DateTime availableFromDateTime, DateTime? availableUntilDateTime)
        {
            return _context.Products
                .Where(ProductExtensions.IsAvailable(availableFromDateTime, availableUntilDateTime))
                .MapToResults(availableFromDateTime, availableUntilDateTime)
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
