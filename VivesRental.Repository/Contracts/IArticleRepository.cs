using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAll();
        IEnumerable<Article> GetAll(ArticleIncludes includes);

        IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate);
        IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate, ArticleIncludes includes);

        Article Get(Guid id);
        Article Get(Guid id, ArticleIncludes includes);

        void Remove(Guid id);
        void Add(Article article);
    }
}
