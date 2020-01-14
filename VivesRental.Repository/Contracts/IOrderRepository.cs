using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Results;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderRepository
    {
        Order Get(Guid id, OrderIncludes includes = null);
        IEnumerable<Order> GetAll(OrderIncludes includes = null);
        IEnumerable<OrderResult> GetAllResult(OrderIncludes includes = null);
        IEnumerable<OrderResult> FindResult(Expression<Func<Order, bool>> predicate, OrderIncludes includes = null);
        void Add(Order order);
        bool ClearCustomer(Guid customerId);
    }
}
