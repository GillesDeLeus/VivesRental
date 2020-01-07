using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Repository
{
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly IVivesRentalDbContext _context;

        public OrderLineRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public OrderLine Get(Guid id)
        {
            var query = _context.OrderLines
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = query.Where(i => i.Id == id); //Add the where clause
            return query.FirstOrDefault();
        }

        public IEnumerable<OrderLine> Find(Expression<Func<OrderLine, bool>> predicate)
        {
            return _context.OrderLines
                .Where(predicate).AsQueryable();
        }

        public void Add(OrderLine orderLine)
        {
            _context.OrderLines.Add(orderLine);
        }

    }
}
