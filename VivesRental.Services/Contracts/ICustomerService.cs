using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerResult> GetAsync(Guid id);
	    Task<List<CustomerResult>> AllAsync();
        Task<CustomerResult> CreateAsync(Customer entity);
        Task<CustomerResult> EditAsync(Customer entity);
        Task<bool> RemoveAsync(Guid id);
    }
}
