using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderLineRepository
    {
        OrderLine Get(Guid id);
        IEnumerable<OrderLine> Find(Expression<Func<OrderLine, bool>> predicate);
        void Add(OrderLine orderLine);
    }
}
