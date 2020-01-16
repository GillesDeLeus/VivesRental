using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> Find();
        IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate);
        void Add(Customer customer);
        void Remove(Guid id);
    }
}
