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
	    IList<Article> GetRentedArticles();
	    IList<Article> GetRentedArticles(Guid customerId);

		Article Create(Article entity);
        Article Edit(Article entity);
        bool Remove(Guid id);
    }
}
