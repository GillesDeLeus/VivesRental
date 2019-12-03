using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface ICustomerService
    {
        Customer Get(Guid id);
	    IList<Customer> All();
        Customer Create(Customer entity);
        Customer Edit(Customer entity);
        bool Remove(Guid id);
    }
}
