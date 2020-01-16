using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleReservationRepository
    {
        void Remove(Guid id);
        void RemoveByArticleId(Guid articleId);
        void RemoveByProductId(Guid productId);
        void Add(ArticleReservation article);
        IEnumerable<ArticleReservation> Find(ArticleReservationIncludes includes = null);
        IEnumerable<ArticleReservation> Find(Expression<Func<ArticleReservation, bool>> predicate, ArticleReservationIncludes includes = null);
    }
}