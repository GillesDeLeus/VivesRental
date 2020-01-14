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

        public IList<Article> GetAvailableArticles()
        {
            return _unitOfWork.Articles.Find(a => a.Status == ArticleStatus.Normal &&
                                                         a.OrderLines.All(ol => ol.ReturnedAt.HasValue)).ToList();
        }

        public IList<Article> GetAvailableArticles(ArticleIncludes includes)
        {
            return _unitOfWork.Articles.Find(a => a.Status == ArticleStatus.Normal &&
                                                         a.OrderLines.All(ol => ol.ReturnedAt.HasValue), includes).ToList();
        }

        public IList<Article> GetAvailableArticles(Guid productId)
        {
            return _unitOfWork.Articles.Find(a => a.ProductId == productId &&
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue)).ToList();
        }

        public IList<Article> GetAvailableArticles(Guid productId, ArticleIncludes includes)
        {
            return _unitOfWork.Articles.Find(a => a.ProductId == productId && 
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue), includes).ToList();
        }

        public IList<Article> GetRentedArticles()
        {
            return _unitOfWork.Articles.Find(ri => ri.Status == ArticleStatus.Normal &&
                                                         ri.OrderLines.Any(rol => !rol.ReturnedAt.HasValue)).ToList();
        }

        public IList<Article> GetRentedArticles(ArticleIncludes includes)
        {
            return _unitOfWork.Articles.Find(ri => ri.Status == ArticleStatus.Normal &&
                                                         ri.OrderLines.Any(rol => !rol.ReturnedAt.HasValue), includes).ToList();
        }

        public IList<Article> GetRentedArticles(Guid customerId)
        {
            return _unitOfWork.Articles.Find(ri => ri.Status == ArticleStatus.Normal &&
                                                         ri.OrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.Order.CustomerId == customerId)).ToList();
        }

        public IList<Article> GetRentedArticles(Guid customerId, ArticleIncludes includes)
        {
            return _unitOfWork.Articles.Find(ri => ri.Status == ArticleStatus.Normal &&
                                                         ri.OrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.Order.CustomerId == customerId), includes).ToList();
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

        public bool Remove(Guid id)
        {
            var article = _unitOfWork.Articles.Get(id, new ArticleIncludes { OrderLines = true });
            if (article == null)
                return false;

            foreach (var orderLine in article.OrderLines)
            {
                orderLine.Article = null;
                orderLine.ArticleId = null;
            }

            _unitOfWork.Articles.Remove(article.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

    }
}
