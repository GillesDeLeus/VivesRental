using System;
using System.Collections.Generic;
using System.Linq;
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

        public OrderResult Get(Guid id, OrderIncludes includes = null)
        {
            return _context.Orders
                .AddIncludes(includes)
                .Where(o => o.Id == id)
                .MapToResults()
                .FirstOrDefault();
        }

        public IList<OrderResult> FindByCustomerId(Guid customerId, OrderIncludes includes = null)
        {
            return _context.Orders
                .AddIncludes(includes)
                .Where(o => o.CustomerId == customerId)
                .MapToResults()
                .ToList();
        }

        public IList<OrderResult> All()
        {
            return _context.Orders
                .MapToResults()
                .ToList();
        }
        
        public OrderResult Create(Guid customerId)
        {
            var customer = _context.Customers
                .Where(c => c.Id == customerId)
                .MapToResults()
                .FirstOrDefault();

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
            var numberOfObjectsUpdated = _context.SaveChanges();
            if (numberOfObjectsUpdated > 0)
            {
                return order.MapToResult();
            }
            return null;
        }

        public bool Return(Guid orderId, DateTime returnedAt)
        {
            var orderLines = _context.OrderLines
                .Where(ol => ol.OrderId == orderId && !ol.ReturnedAt.HasValue)
                .ToList();
            foreach (var orderLine in orderLines)
            {
                orderLine.ReturnedAt = returnedAt;
            }

            var numberOfObjectsUpdated = _context.SaveChanges();
            return numberOfObjectsUpdated > 0;
        }
    }
}
