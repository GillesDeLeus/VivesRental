using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Contracts;

namespace VivesRental.Services
{

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Order Get(Guid id)
        {
            return _unitOfWork.Orders.Get(id);
        }

        public IList<Order> All()
        {
            return _unitOfWork.Orders.GetAll().ToList();
        }

        public Order Create(Guid customerId)
        {
            var customer = _unitOfWork.Customers.Get(customerId);

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
                return order;
            }
            return null;
        }

        public bool Return(Guid orderId, DateTime returnedAt)
        {
            var orderLines = _unitOfWork.OrderLines.Find(rol => rol.OrderId == orderId);
            foreach (var orderLine in orderLines)
            {
                orderLine.ReturnedAt = returnedAt;
            }

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
