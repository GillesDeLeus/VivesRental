using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{

    public class ArticleReservationService : IArticleReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ArticleReservationResult Get(Guid id)
        {
            return _unitOfWork.ArticleReservations
                .Where(ar => ar.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }

        public ArticleReservationResult Get(Guid id, ArticleReservationIncludes includes)
        {
            return _unitOfWork.ArticleReservations
                .Where(ar => ar.Id == id, includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }

        public IList<ArticleReservationResult> All()
        {
            return _unitOfWork.ArticleReservations
                .Where()
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleReservationResult> All(ArticleReservationIncludes includes)
        {
            return _unitOfWork.ArticleReservations
                .Where(includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public ArticleReservationResult Create(Guid customerId, Guid articleId)
        {
            var articleReservation = new ArticleReservation
            {
                ArticleId = articleId,
                CustomerId = customerId,
                FromDateTime = DateTime.Now,
                UntilDateTime = DateTime.Now.AddMinutes(5)
            };
            return Create(articleReservation);
        }

        public ArticleReservationResult Create(ArticleReservation entity)
        {

            if (!entity.IsValid())
            {
                return null;
            }

            var articleReservation = new ArticleReservation
            {
                CustomerId = entity.CustomerId,
                ArticleId = entity.ArticleId,
                FromDateTime = entity.FromDateTime,
                UntilDateTime = entity.UntilDateTime
            };

            _unitOfWork.ArticleReservations.Add(articleReservation);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                //Detach and return
                return articleReservation.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        /// <summary>
        /// Removes one ArticleReservation
        /// </summary>
        /// <param name="id">The id of the ArticleReservation</param>
        /// <returns>True if the article reservation was deleted</returns>
        public bool Remove(Guid id)
        {
            var article = _unitOfWork.ArticleReservations
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if (article == null)
                return false;

            _unitOfWork.ArticleReservations.Remove(article.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
