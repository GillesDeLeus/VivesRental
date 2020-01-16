using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IProductService
    {
        Task<ProductResult> GetAsync(Guid id);
        Task<List<ProductResult>> AllAsync();
        Task<List<ProductResult>> AllAsync(DateTime fromDateTime, DateTime untilDateTime);
        Task<ProductResult> CreateAsync(Product entity);
        Task<ProductResult> EditAsync(Product entity);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> GenerateArticlesAsync(Guid productId, int amount);
        Task<List<ProductResult>> GetAvailableProductResultsAsync();
        Task<List<ProductResult>> GetAvailableProductResultsAsync(DateTime fromDateTime, DateTime untilDateTime);

    }
}
