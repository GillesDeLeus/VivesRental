using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderService
    {
        Order Get(Guid id);
        IList<Order> All();
        IList<OrderResult> AllResult();

        Order Create(Guid customerId);
		bool Return(Guid id, DateTime returnedAt);
    }
}
