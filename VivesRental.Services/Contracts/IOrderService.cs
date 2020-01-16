using System;
using System.Collections.Generic;
using VivesRental.Repository.Includes;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderService
    {
        OrderResult Get(Guid id, OrderIncludes includes = null);

        IList<OrderResult> FindByCustomerId(Guid customerId, OrderIncludes includes = null);
        IList<OrderResult> All();

        OrderResult Create(Guid customerId);
		bool Return(Guid id, DateTime returnedAt);
    }
}
