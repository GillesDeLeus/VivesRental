using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;

namespace VivesRental.Services
{
    
    public class ArticleReservationService : IArticleReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ArticleReservation Get(Guid id)
        {
            return _unitOfWork.ArticleReservations.Get(id);
        }

        public ArticleReservation Get(Guid id, ArticleReservationIncludes includes)
        {
            return _unitOfWork.ArticleReservations.Get(id, includes);
        }

        public IList<ArticleReservation> All()
        {
            return _unitOfWork.ArticleReservations.GetAll().ToList();
        }

        public IList<ArticleReservation> All(ArticleReservationIncludes includes)
        {
            return _unitOfWork.ArticleReservations.GetAll(includes).ToList();
        }

        public ArticleReservation Create(Guid customerId, Guid articleId)
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

        public ArticleReservation Create(ArticleReservation entity)
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
                return articleReservation;
            }
            return null;
        }
        
        public bool Remove(Guid id)
        {
            var article = _unitOfWork.ArticleReservations.Get(id);
            if (article == null)
                return false;

            _unitOfWork.ArticleReservations.Remove(article.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
