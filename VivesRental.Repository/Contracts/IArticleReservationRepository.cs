using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IArticleReservationRepository
    {
        ArticleReservation Get(Guid id, ArticleReservationIncludes includes = null);
        void Remove(Guid id);
        void Add(ArticleReservation article);
        IEnumerable<ArticleReservation> Find(Expression<Func<ArticleReservation, bool>> predicate, ArticleReservationIncludes includes = null);
        IEnumerable<ArticleReservation> GetAll(ArticleReservationIncludes includes = null);
    }
}