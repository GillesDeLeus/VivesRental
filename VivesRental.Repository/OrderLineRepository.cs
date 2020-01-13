using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository
{
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly IVivesRentalDbContext _context;

        public OrderLineRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public OrderLine Get(Guid id, OrderLineIncludes includes = null)
        {
            return _context.OrderLines
                .AddIncludes(includes)//Add the where clause
                .FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<OrderLine> Find(Expression<Func<OrderLine, bool>> predicate, OrderLineIncludes includes = null)
        {
            return _context.OrderLines
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public void Add(OrderLine orderLine)
        {
            _context.OrderLines.Add(orderLine);
        }

    }
}
