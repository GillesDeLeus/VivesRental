using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IArticleService
    {
        Task<ArticleResult> GetAsync(Guid id, ArticleIncludes includes = null);
        Task<List<ArticleResult>> AllAsync(ArticleIncludes includes = null);
        Task<List<ArticleResult>> GetAvailableArticlesAsync(ArticleIncludes includes = null);
        Task<List<ArticleResult>> GetAvailableArticlesAsync(Guid productId, ArticleIncludes includes = null);
        Task<List<ArticleResult>> GetRentedArticlesAsync(ArticleIncludes includes = null);

        Task<List<ArticleResult>> GetRentedArticlesAsync(Guid customerId, ArticleIncludes includes = null);

        Task<ArticleResult> CreateAsync(Article entity);
       
        Task<bool> UpdateStatusAsync(Guid articleId, ArticleStatus status);
        Task<bool> RemoveAsync(Guid id);

        Task<bool> IsAvailableAsync(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null);
        Task<bool> IsAvailableAsync(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null);
    }
}
