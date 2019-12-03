using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Customer Get(Guid id);
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
        void Remove(Guid id);
    }
}
