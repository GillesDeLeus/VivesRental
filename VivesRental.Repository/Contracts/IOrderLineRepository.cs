using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderLineRepository
    {
        OrderLine Get(Guid id, OrderLineIncludes includes = null);
        IEnumerable<OrderLine> Find(Expression<Func<OrderLine, bool>> predicate, OrderLineIncludes includes = null);
        void Add(OrderLine orderLine);
    }
}
