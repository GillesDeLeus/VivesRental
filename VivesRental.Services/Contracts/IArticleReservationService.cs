using System;
using System.Collections.Generic;
using VivesRental.Model;
using VivesRental.Repository.Includes;
using VivesRental.Services.Results;

namespace VivesRental.Services.Contracts
{
    public interface IArticleReservationService
    {
        ArticleReservationResult Get(Guid id);
        ArticleReservationResult Get(Guid id, ArticleReservationIncludes includes);
        IList<ArticleReservationResult> All();
        IList<ArticleReservationResult> All(ArticleReservationIncludes includes);
        ArticleReservationResult Create(Guid customerId, Guid articleId);
        ArticleReservationResult Create(ArticleReservation entity);
        bool Remove(Guid id);
    }
}
