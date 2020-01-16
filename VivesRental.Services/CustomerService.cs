using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IVivesRentalDbContext _context;

        public CustomerService(IVivesRentalDbContext context)
        {
            _context = context;
        }


        public CustomerResult Get(Guid id)
        {
            return _context.Customers
                .Where(c => c.Id == id)
                .MapToResults()
                .FirstOrDefault();
        }

        public IList<CustomerResult> All()
        {
            return _context.Customers
                .MapToResults()
                .ToList();
        }

        public CustomerResult Create(Customer entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            var customer = new Customer
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber
            };

            _context.Customers.Add(customer);
            var numberOfObjectsUpdated = _context.SaveChanges();

            if (numberOfObjectsUpdated > 0)
                return customer.MapToResult();

            return null;
        }

        public CustomerResult Edit(Customer entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var customer = _context.Customers
                .FirstOrDefault(c => c.Id==entity.Id);

            if (customer == null)
            {
                return null;
            }

            //Only update the properties we want to update
            customer.FirstName = entity.FirstName;
            customer.LastName = entity.LastName;
            customer.Email = entity.Email;
            customer.PhoneNumber = entity.PhoneNumber;

            var numberOfObjectsUpdated = _context.SaveChanges();
            if (numberOfObjectsUpdated > 0)
                return entity.MapToResult();
            return null;
        }

        /// <summary>
        /// Removes one Customer and disconnects Orders from the customer
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <returns>True if the customer was deleted</returns>
        public bool Remove(Guid id)
        {
            var result = _context.RunInTransaction(() =>
            {
                //Remove the Customer from the Orders
                ClearCustomer(id);
                //Remove the Order
                _context.Customers.Remove(id);

                var numberOfObjectsUpdated = _context.SaveChangesWithConcurrencyIgnore();

                return numberOfObjectsUpdated > 0;
            });
            return result;
        }

        private void ClearCustomer(Guid customerId)
        {
            if (_context.Database.IsInMemory())
            {
                var orders = _context.Orders.Where(ol => ol.CustomerId == customerId).ToList();
                foreach (var order in orders)
                {
                    order.Customer = null;
                    order.CustomerId = null;
                }
                return;
            }

            var commandText = "UPDATE [Order] SET CustomerId = null WHERE CustomerId = @CustomerId";
            var customerIdParameter = new SqlParameter("@CustomerId", customerId);

            _context.Database.ExecuteSqlRaw(commandText, customerIdParameter);
        }
    }
}
