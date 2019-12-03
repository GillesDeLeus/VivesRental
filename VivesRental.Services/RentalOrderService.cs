using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Contracts;

namespace VivesRental.Services
{

    public class RentalOrderService : IRentalOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Order Get(Guid id)
        {
            return _unitOfWork.RentalOrders.Get(id);
        }

        public IList<Order> All()
        {
            return _unitOfWork.RentalOrders.GetAll().ToList();
        }

        public Order Create(Guid customerId)
        {
            var customer = _unitOfWork.Customers.Get(customerId);
            if (customer == null)
            {
                return null;
            }

            var rentalOrder = new Order
            {
                CustomerId = customer.Id,
                CustomerFirstName = customer.FirstName,
                CustomerLastName = customer.Name,
                CustomerEmail = customer.Email,
                CreatedAt = DateTime.Now
            };

            _unitOfWork.RentalOrders.Add(rentalOrder);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return rentalOrder;
            }
            return null;
        }

        public bool Return(Guid rentalOrderId, DateTime returnedAt)
        {
            var rentalOrderLines = _unitOfWork.RentalOrderLines.Find(rol => rol.RentalOrderId == rentalOrderId);
            foreach (var rentalOrderLine in rentalOrderLines)
            {
                rentalOrderLine.ReturnedAt = returnedAt;
            }

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
