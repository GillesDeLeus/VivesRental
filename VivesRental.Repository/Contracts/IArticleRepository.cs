using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleRepository
    {
         IEnumerable<Article> GetAll(ArticleIncludes includes = null);

        IEnumerable<Article> Find(Func<Article, bool> predicate, ArticleIncludes includes = null);

        bool All(Guid articleId, Func<Article, bool> predicate);
        bool All(IList<Guid> articleIds, Func<Article, bool> predicate);

        Article Get(Guid id, ArticleIncludes includes = null);

        void Remove(Guid id);
        void Add(Article article);
    }
}
