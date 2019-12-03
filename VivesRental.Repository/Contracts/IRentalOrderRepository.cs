using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface IRentalOrderRepository
    {
        Order Get(Guid id);
        IEnumerable<Order> GetAll();
        void Add(Order rentalOrder);
    }
}
