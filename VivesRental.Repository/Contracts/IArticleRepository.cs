using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleRepository
    {
        IEnumerable<Article> Where(ArticleIncludes includes = null);
        IEnumerable<Article> Where(Expression<Func<Article, bool>> predicate, ArticleIncludes includes = null);

        bool All(Guid articleId, Expression<Func<Article, bool>> predicate);
        bool All(IList<Guid> articleIds, Expression<Func<Article, bool>> predicate);

        void Remove(Guid id);
        void RemoveByProductId(Guid productId);
        void Add(Article article);
    }
}
