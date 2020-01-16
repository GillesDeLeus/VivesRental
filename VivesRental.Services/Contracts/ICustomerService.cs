using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface ICustomerService
    {
        CustomerResult Get(Guid id);
	    IList<CustomerResult> All();
        CustomerResult Create(Customer entity);
        CustomerResult Edit(Customer entity);
        bool Remove(Guid id);
    }
}
