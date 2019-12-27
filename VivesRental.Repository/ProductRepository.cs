using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;

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
		    var query = _context.Products
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
		    query = query.Where(i => i.Id == id); //Add the where clause
		    return query.FirstOrDefault(); 
	    }

        public IEnumerable<Product> GetAll(ProductIncludes includes = null)
		{
			var query = _context.Products
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
			query = AddIncludes(query, includes);
			return query.AsEnumerable(); 
		}

		public IEnumerable<Product> Find(Expression<Func<Product, bool>> predicate, ProductIncludes includes = null)
		{
			var query = _context.Products
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
			query = AddIncludes(query, includes);
			return query.Where(predicate).AsEnumerable(); //Add the where clause and return IEnumerable<Product>
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



        /// <summary>
        /// Adds the DbContext includes based on the booleans set in the Includes object
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        private IQueryable<Product> AddIncludes(IQueryable<Product> query, ProductIncludes includes)
	    {
		    if (includes == null)
			    return query;

		    if (includes.Articles)
			    query = query.Include(i => i.Articles);

            if (includes.ArticleOrderLines)
            {
	            query = query
                    .Include(p => p.Articles)
                    .ThenInclude(a => a.OrderLines);
            }

		    return query;
	    }

       
    }
}
