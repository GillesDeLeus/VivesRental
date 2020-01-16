using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public async Task<CustomerResult> GetAsync(Guid id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }

        public async Task<List<CustomerResult>> AllAsync()
        {
            return await _context.Customers
                .MapToResults()
                .ToListAsync();
        }

        public async Task<CustomerResult> CreateAsync(Customer entity)
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
            var numberOfObjectsUpdated = await _context.SaveChangesAsync();

            if (numberOfObjectsUpdated > 0)
                return customer.MapToResult();

            return null;
        }

        public async Task<CustomerResult> EditAsync(Customer entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id==entity.Id);

            if (customer == null)
            {
                return null;
            }

            //Only update the properties we want to update
            customer.FirstName = entity.FirstName;
            customer.LastName = entity.LastName;
            customer.Email = entity.Email;
            customer.PhoneNumber = entity.PhoneNumber;

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
                return entity.MapToResult();
            return null;
        }

        /// <summary>
        /// Removes one Customer and disconnects Orders from the customer
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <returns>True if the customer was deleted</returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _context.RunInTransactionAsync(async () =>
            {
                //Remove the Customer from the Orders
                await ClearCustomerAsync(id);
                //Remove the Order
                _context.Customers.Remove(id);

                var numberOfObjectsUpdated = await _context.SaveChangesWithConcurrencyIgnoreAsync();

                return numberOfObjectsUpdated > 0;
            });
            return await result;
        }

        private async Task ClearCustomerAsync(Guid customerId)
        {
            if (_context.Database.IsInMemory())
            {
                var orders = await _context.Orders.Where(ol => ol.CustomerId == customerId).ToListAsync();
                foreach (var order in orders)
                {
                    order.Customer = null;
                    order.CustomerId = null;
                }
                return;
            }

            var commandText = "UPDATE [Order] SET CustomerId = null WHERE CustomerId = @CustomerId";
            var customerIdParameter = new SqlParameter("@CustomerId", customerId);

            await _context.Database.ExecuteSqlRawAsync(commandText, customerIdParameter);
        }
    }
}
