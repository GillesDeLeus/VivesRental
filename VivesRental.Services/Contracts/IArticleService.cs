using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IArticleService
    {
        ArticleResult Get(Guid id, ArticleIncludes includes = null);
        IList<ArticleResult> All(ArticleIncludes includes = null);
        IList<ArticleResult> GetAvailableArticles(ArticleIncludes includes = null);
        IList<ArticleResult> GetAvailableArticles(Guid productId, ArticleIncludes includes = null);
        IList<ArticleResult> GetRentedArticles(ArticleIncludes includes = null);

        IList<ArticleResult> GetRentedArticles(Guid customerId, ArticleIncludes includes = null);

        ArticleResult Create(Article entity);
       
        bool UpdateStatus(Guid articleId, ArticleStatus status);
        bool Remove(Guid id);

        bool IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null);
        bool IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null);
    }
}
