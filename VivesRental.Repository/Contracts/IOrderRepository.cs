using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderRepository
    {
        Order Get(Guid id);
        IEnumerable<Order> GetAll();
        IEnumerable<OrderResult> GetAllResult();
        void Add(Order order);
    }
}
