using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Find(OrderIncludes includes = null);
        IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate, OrderIncludes includes = null);
        void Add(Order order);

        void ClearCustomer(Guid customerId);
    }
}
