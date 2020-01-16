using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderResult Get(Guid id, OrderIncludes includes = null)
        {
            return _unitOfWork.Orders
                .Find(o => o.Id == id, includes)
                .MapToResults()
                .FirstOrDefault();
        }

        public IList<OrderResult> FindByCustomerId(Guid customerId, OrderIncludes includes = null)
        {
            return _unitOfWork.Orders
                .Find(o => o.CustomerId == customerId, includes)
                .MapToResults()
                .ToList();
        }

        public IList<OrderResult> All()
        {
            return _unitOfWork.Orders
                .Find()
                .MapToResults()
                .ToList();
        }
        
        public OrderResult Create(Guid customerId)
        {
            var customer = _unitOfWork.Customers.Find(c => c.Id == customerId)
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

            _unitOfWork.Orders.Add(order);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return order.MapToResult();
            }
            return null;
        }

        public bool Return(Guid orderId, DateTime returnedAt)
        {
            var orderLines = _unitOfWork.OrderLines.Find(ol => ol.OrderId == orderId && !ol.ReturnedAt.HasValue);
            foreach (var orderLine in orderLines)
            {
                orderLine.ReturnedAt = returnedAt;
            }

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
