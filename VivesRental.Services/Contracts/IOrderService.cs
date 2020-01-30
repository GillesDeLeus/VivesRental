using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IOrderService
    {
        Task<OrderResult> GetAsync(Guid id);

        Task<List<OrderResult>> FindByCustomerIdAsync(Guid customerId);
        Task<List<OrderResult>> AllAsync();

        Task<OrderResult> CreateAsync(Guid customerId);
		Task<bool> ReturnAsync(Guid id, DateTime returnedAt);
    }
}
