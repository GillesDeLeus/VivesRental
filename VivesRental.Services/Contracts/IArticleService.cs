using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Services.Filters;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IArticleService
    {
        Task<ArticleResult> GetAsync(Guid id);

        [Obsolete("This method is obsolete, use FindAsync in stead", false)]
        Task<List<ArticleResult>> AllAsync();

        Task<List<ArticleResult>> FindAsync(ArticleFilter filter = null);
        Task<List<ArticleResult>> GetAvailableArticlesAsync();
        Task<List<ArticleResult>> GetAvailableArticlesAsync(Guid productId);
        Task<List<ArticleResult>> GetRentedArticlesAsync();

        Task<List<ArticleResult>> GetRentedArticlesAsync(Guid customerId);

        Task<ArticleResult> CreateAsync(Article entity);
       
        Task<bool> UpdateStatusAsync(Guid articleId, ArticleStatus status);
        Task<bool> RemoveAsync(Guid id);

        Task<bool> IsAvailableAsync(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null);
        Task<bool> IsAvailableAsync(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null);
    }
}
