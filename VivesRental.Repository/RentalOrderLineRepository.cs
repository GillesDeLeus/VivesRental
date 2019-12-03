using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Repository
{
    public class RentalOrderLineRepository : IRentalOrderLineRepository
    {
        private readonly IVivesRentalDbContext _context;

        public RentalOrderLineRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public RentalOrderLine Get(Guid id)
        {
            var query = _context.RentalOrderLines.AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = query.Where(i => i.Id == id); //Add the where clause
            return query.FirstOrDefault();
        }

        public IEnumerable<RentalOrderLine> Find(Expression<Func<RentalOrderLine, bool>> predicate)
        {
            return _context.RentalOrderLines.Where(predicate).AsQueryable();
        }

        public void Add(RentalOrderLine rentalOrderLine)
        {
            _context.RentalOrderLines.Add(rentalOrderLine);
        }
    }
}
