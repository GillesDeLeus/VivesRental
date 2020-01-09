using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Services.Contracts
{
    public interface IArticleReservationService
    {
        ArticleReservation Get(Guid id);
        ArticleReservation Get(Guid id, ArticleReservationIncludes includes);
        IList<ArticleReservation> All();
        IList<ArticleReservation> All(ArticleReservationIncludes includes);
        ArticleReservation Create(Guid customerId, Guid articleId);
        ArticleReservation Create(ArticleReservation entity);
        bool Remove(Guid id);
    }
}
