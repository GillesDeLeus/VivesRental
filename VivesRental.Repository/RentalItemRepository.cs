using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository
{
    public class RentalItemRepository : IRentalItemRepository
    {
        private readonly IVivesRentalDbContext _context;
        public RentalItemRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public RentalItem Get(Guid id)
        {
            return Get(id, null);
        }

        public RentalItem Get(Guid id, RentalItemIncludes includes)
        {
            var query = _context.RentalItems.AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            query = query.Where(i => i.Id == id); //Add the where clause
            return query.FirstOrDefault();
        }

        public void Remove(Guid id)
        {
            var entity = new RentalItem { Id = id };
            _context.RentalItems.Attach(entity);
            _context.RentalItems.Remove(entity);
        }

        public void Add(RentalItem rentalItem)
        {
            _context.RentalItems.Add(rentalItem);
        }

        public IEnumerable<RentalItem> Find(Expression<Func<RentalItem, bool>> predicate)
        {
            return Find(predicate, null);
        }

        public IEnumerable<RentalItem> Find(Expression<Func<RentalItem, bool>> predicate, RentalItemIncludes includes)
        {
            var query = _context.RentalItems.AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.Where(predicate).AsEnumerable(); //Add the where clause and return IEnumerable<RentalItem>
        }

        public IEnumerable<RentalItem> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<RentalItem> GetAll(RentalItemIncludes includes)
        {
            var query = _context.RentalItems.AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.AsEnumerable();
        }

        private IQueryable<RentalItem> AddIncludes(IQueryable<RentalItem> query, RentalItemIncludes includes)
        {
            if (includes == null)
                return query;

            if (includes.Item)
                query = query.Include(i => i.Item);

	        if (includes.RentalOrderLines)
		        query = query.Include(i => i.RentalOrderLines);

            return query;
        }
    }
}
