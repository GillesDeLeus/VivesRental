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
        IList<Article> GetAvailableArticles();
        IList<Article> GetAvailableArticles(ArticleIncludes includes);
        IList<Article> GetAvailableArticles(Guid productId);
        IList<Article> GetAvailableArticles(Guid productId, ArticleIncludes includes);
        IList<Article> GetRentedArticles();
        IList<Article> GetRentedArticles(ArticleIncludes includes);

        IList<Article> GetRentedArticles(Guid customerId);
        IList<Article> GetRentedArticles(Guid customerId, ArticleIncludes includes);

        Article Create(Article entity);
        [Obsolete("Edit has been replaced by the UpdateStatus method. Use the UpdateStatus method in stead.")]
        Article Edit(Article entity);
        bool UpdateStatus(Guid articleId, ArticleStatus status);
        bool Remove(Guid id);
    }
}
