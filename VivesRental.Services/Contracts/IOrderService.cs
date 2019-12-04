using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IOrderService
    {
        Order Get(Guid id);
	    IList<Order> All();
	    Order Create(Guid customerId);
		bool Return(Guid id, DateTime returnedAt);
    }
}
