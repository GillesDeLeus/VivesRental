using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public CustomerResult Get(Guid id)
        {
            return _unitOfWork.Customers
                .Where(c => c.Id == id)
                .MapToResults()
                .FirstOrDefault();
        }

        public IList<CustomerResult> All()
        {
            return _unitOfWork.Customers
                .Where()
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

            _unitOfWork.Customers.Add(customer);
            var numberOfObjectsUpdated = _unitOfWork.Complete();

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
            var customer = _unitOfWork.Customers
                .Where(c => c.Id==entity.Id)
                .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            //Only update the properties we want to update
            customer.FirstName = entity.FirstName;
            customer.LastName = entity.LastName;
            customer.Email = entity.Email;
            customer.PhoneNumber = entity.PhoneNumber;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
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
            var customer = _unitOfWork.Customers
                .Where(c => c.Id == id)
                .FirstOrDefault();

            if (customer == null)
                return false;

            _unitOfWork.BeginTransaction();
            //Remove the Customer from the Orders
            _unitOfWork.Orders.ClearCustomer(id);
            //Remove the Order
            _unitOfWork.Customers.Remove(id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
