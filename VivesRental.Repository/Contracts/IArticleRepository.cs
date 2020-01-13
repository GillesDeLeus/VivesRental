using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAll(ArticleIncludes includes = null);

        IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate, ArticleIncludes includes = null);

        Article Get(Guid id, ArticleIncludes includes = null);

        void Remove(Guid id);
        void Add(Article article);
    }
}
