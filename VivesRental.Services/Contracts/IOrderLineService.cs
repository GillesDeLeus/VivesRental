using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderLineService
    {
        Task<OrderLineResult> GetAsync(Guid id);
        Task<bool> RentAsync(Guid orderId, Guid articleId);
        Task<bool> RentAsync(Guid orderId, IList<Guid> articleIds);
        Task<bool> ReturnAsync(Guid orderLineId, DateTime returnedAt);
	    Task<List<OrderLineResult>> FindByOrderIdAsync(Guid orderId);

    }
}
