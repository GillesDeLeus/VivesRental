using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{

    public class OrderService : IOrderService
    {
        private readonly IVivesRentalDbContext _context;

        public OrderService(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResult> GetAsync(Guid id, OrderIncludes includes = null)
        {
            return await _context.Orders
                .AddIncludes(includes)
                .Where(o => o.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderResult>> FindByCustomerIdAsync(Guid customerId, OrderIncludes includes = null)
        {
            return await _context.Orders
                .AddIncludes(includes)
                .Where(o => o.CustomerId == customerId)
                .MapToResults()
                .ToListAsync();
        }

        public async Task<List<OrderResult>> AllAsync()
        {
            return await _context.Orders
                .MapToResults()
                .ToListAsync();
        }
        
        public async Task<OrderResult> CreateAsync(Guid customerId)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == customerId)
                .MapToResults()
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                return null;
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                CustomerFirstName = customer.FirstName,
                CustomerLastName = customer.LastName,
                CustomerEmail = customer.Email,
                CustomerPhoneNumber = customer.PhoneNumber,
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
            {
                return order.MapToResult();
            }
            return null;
        }

        public async Task<bool> ReturnAsync(Guid orderId, DateTime returnedAt)
        {
            var orderLines = await _context.OrderLines
                .Where(ol => ol.OrderId == orderId && !ol.ReturnedAt.HasValue)
                .ToListAsync();
            foreach (var orderLine in orderLines)
            {
                orderLine.ReturnedAt = returnedAt;
            }

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            return numberOfObjectsUpdated > 0;
        }
    }
}
