using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;
using VivesRental.Services.Mappers;
using VivesRental.Services.Results;

namespace VivesRental.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ArticleResult Get(Guid id, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(a => a.Id == id, includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }
        
        public IList<ArticleResult> All(ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(includes)
                .MapToResults(DateTime.Now,DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetAvailableArticles(ArticleIncludes includes = null)
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;

            return GetAvailableArticles(fromDateTime, untilDateTime, includes);
        }

        public IList<ArticleResult> GetAvailableArticles(DateTime fromDateTime, DateTime untilDateTime, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime), includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }
        
        public IList<ArticleResult> GetAvailableArticles(Guid productId, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(a => a.ProductId == productId &&
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue), includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetRentedArticles(ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(a => a.IsRented(), includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetRentedArticles(Guid customerId, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles
                .Find(a => a.IsRented(customerId), includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public ArticleResult Create(Article entity)
        {

            if (!entity.IsValid())
            {
                return null;
            }

            var article = new Article
            {
                ProductId = entity.ProductId,
                Status = entity.Status
            };

            _unitOfWork.Articles.Add(article);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                //Detach and return
                return article.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        public bool UpdateStatus(Guid articleId, ArticleStatus status)
        {
            //Get Product from unitOfWork
            var article = _unitOfWork.Articles
                .Find(a => a.Id== articleId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();

            if (article == null)
            {
                return false;
            }

            //Only update the properties we want to update
            article.Status = status;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Removes one Article, Removes the ArticleReservations and disconnects OrderLines from the Article
        /// </summary>
        /// <param name="id">The id of the Article</param>
        /// <returns>True if the article was deleted</returns>
        public bool Remove(Guid id)
        {
            var article = _unitOfWork.Articles
                .Find(a => a.Id == id)
                .FirstOrDefault();

            if (article == null)
                return false;

            _unitOfWork.BeginTransaction();
            _unitOfWork.OrderLines.ClearArticleByArticleId(id);
            _unitOfWork.ArticleReservations.RemoveByArticleId(id);
            _unitOfWork.Articles.Remove(article.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        public bool IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _unitOfWork.Articles
                .All(articleId, ArticleExtensions.IsAvailable(articleId, fromDateTime, untilDateTime));
        }
        
        public bool IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _unitOfWork.Articles
                .All(articleIds, ArticleExtensions.IsAvailable(articleIds, fromDateTime, untilDateTime));
        }

}
}
