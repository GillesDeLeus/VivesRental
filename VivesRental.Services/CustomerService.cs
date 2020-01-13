using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;

namespace VivesRental.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Customer Get(Guid id)
        {
            return _unitOfWork.Customers.Get(id);
        }

        public IList<Customer> All()
        {
            return _unitOfWork.Customers.GetAll().ToList();
        }

        public Customer Create(Customer entity)
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
                return customer;

            return null;
        }

        public Customer Edit(Customer entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var customer = _unitOfWork.Customers.Get(entity.Id);
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
                return entity;
            return null;
        }

        public bool Remove(Guid id)
        {
            var customer = _unitOfWork.Customers.Get(id);
            if (customer == null)
                return false;

            //Remove the Customer from the Orders
            _unitOfWork.Orders.ClearCustomer(id);
            //Remove the Order
            _unitOfWork.Customers.Remove(id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
