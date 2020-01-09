using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Services.Contracts
{
    public interface IArticleService
    {
        Article Get(Guid id);
        Article Get(Guid id, ArticleIncludes includes);
        IList<Article> All();
        IList<Article> All(ArticleIncludes includes);
        IList<Article> GetAvailableArticles(ArticleIncludes includes = null);

        IList<Article> GetAvailableArticles(DateTime fromDateTime, DateTime untilDateTime,
            ArticleIncludes includes = null);
        IList<Article> GetRentedArticles(ArticleIncludes includes = null);

        IList<Article> GetRentedArticles(Guid customerId, ArticleIncludes includes = null);

        Article Create(Article entity);
        [Obsolete("Edit has been replaced by the UpdateStatus method. Use the UpdateStatus method in stead.")]
        Article Edit(Article entity);
        bool UpdateStatus(Guid articleId, ArticleStatus status);
        bool Remove(Guid id);

        bool IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null);
        bool IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null);
    }
}
