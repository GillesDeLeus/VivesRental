using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IArticleService
    {
        Article Get(Guid id);
	    IList<Article> All();
	    IList<Article> GetAvailableArticles();
	    IList<Article> GetRentedArticles();
	    IList<Article> GetRentedArticles(Guid customerId);

		Article Create(Article entity);
        Article Edit(Article entity);
        bool Remove(Guid id);
    }
}
