using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderService
    {
        Order Get(Guid id, OrderIncludes includes = null);

        IList<OrderResult> FindByCustomerIdResult(Guid customerId, OrderIncludes includes = null);
        IList<Order> All();
        IList<OrderResult> AllResult();

        Order Create(Guid customerId);
		bool Return(Guid id, DateTime returnedAt);
    }
}
