using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;

namespace VivesRental.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Article Get(Guid id)
        {
            return _unitOfWork.Articles.Get(id);
        }

        public Article Get(Guid id, ArticleIncludes includes)
        {
            return _unitOfWork.Articles.Get(id, includes);
        }

        public IList<Article> All()
        {
            return _unitOfWork.Articles.GetAll().ToList();
        }

        public IList<Article> All(ArticleIncludes includes)
        {
            return _unitOfWork.Articles.GetAll(includes).ToList();
        }

        public IList<Article> GetAvailableArticles(ArticleIncludes includes = null)
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;

            return GetAvailableArticles(fromDateTime, untilDateTime, includes);
        }

        public IList<Article> GetAvailableArticles(DateTime fromDateTime, DateTime untilDateTime, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles.Find(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime), includes).ToList();
        }

        public IList<Article> GetRentedArticles(ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles.Find(a => a.IsRented(), includes).ToList();
        }

        public IList<Article> GetRentedArticles(Guid customerId, ArticleIncludes includes = null)
        {
            return _unitOfWork.Articles.Find(a => a.IsRented(customerId), includes).ToList();
        }

        public Article Create(Article entity)
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
                return article;
            }
            return null;
        }

        [Obsolete("Edit has been replaced by the UpdateStatus method. Use the UpdateStatus method in stead.")]
        public Article Edit(Article entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Product from unitOfWork
            var article = _unitOfWork.Articles.Get(entity.Id);
            if (article == null)
            {
                return null;
            }

            //Only update the properties we want to update
            article.ProductId = entity.ProductId;
            article.Status = entity.Status;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return entity;
            }
            return null;
        }

        public bool UpdateStatus(Guid articleId, ArticleStatus status)
        {
            //Get Product from unitOfWork
            var article = _unitOfWork.Articles.Get(articleId);
            if (article == null)
            {
                return false;
            }

            //Only update the properties we want to update
            article.Status = status;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        //TODO: Needs transaction
        public bool Remove(Guid id)
        {
            var article = _unitOfWork.Articles.Get(id);
            if (article == null)
                return false;

            //TODO: SQL Transaction
            _unitOfWork.BeginTransaction();
            _unitOfWork.OrderLines.ClearArticleByArticleId(id);
            _unitOfWork.ArticleReservations.RemoveByArticleId(id);
            _unitOfWork.Articles.Remove(article.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        public bool IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _unitOfWork.Articles.All(articleId, ArticleExtensions.IsAvailable(articleId, fromDateTime, untilDateTime));
        }
        
        public bool IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _unitOfWork.Articles.All(articleIds, ArticleExtensions.IsAvailable(articleIds, fromDateTime, untilDateTime));
        }

}
}
