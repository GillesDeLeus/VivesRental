using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IVivesRentalDbContext _context;

        public CustomerRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public Customer Get(Guid id)
        {
            return _context.Customers.Find(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.AsEnumerable();
        }

        public void Remove(Guid id)
        {
            var entity = new Customer {Id = id};
            _context.Customers.Attach(entity);
            _context.Customers.Remove(entity);
        }
    }
}
