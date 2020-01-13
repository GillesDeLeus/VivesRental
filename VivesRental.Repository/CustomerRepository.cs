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
            var query = _context.Customers
                .AsQueryable();
            return query.SingleOrDefault(c => c.Id== id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers
                .AsEnumerable();
        }

        public void Remove(Guid id)
        {
            var localEntity = _context.Customers.Local.SingleOrDefault(e => e.Id == id);
            if (localEntity == null)
            {
                var entity = new Customer { Id = id };
                _context.Customers.Attach(entity);
                _context.Customers.Remove(entity);
            }
            else
            {
                _context.Customers.Remove(localEntity);
            }
        }
    }
}
