using System;
using System.Collections.Generic;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderLineService
    {
        OrderLineResult Get(Guid id);
        bool Rent(Guid orderId, Guid articleId);
        bool Rent(Guid orderId, IList<Guid> articleIds);
        bool Return(Guid orderLineId, DateTime returnedAt);
	    IList<OrderLineResult> FindByOrderId(Guid orderId);

    }
}
