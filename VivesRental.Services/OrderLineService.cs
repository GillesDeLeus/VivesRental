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
    public class OrderLineService : IOrderLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IArticleService _articleService;

        public OrderLineService(IUnitOfWork unitOfWork, IArticleService articleService)
        {
            _unitOfWork = unitOfWork;
            _articleService = articleService;
        }

        public OrderLineResult Get(Guid id)
        {
            return _unitOfWork.OrderLines
                .Where(ol => ol.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }

        public IList<OrderLineResult> FindByOrderId(Guid orderId)
        {
            return _unitOfWork.OrderLines
                .Where(rol => rol.OrderId == orderId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public bool Rent(Guid orderId, Guid articleId)
        {
            var fromDateTime = DateTime.Now;

            var article = _unitOfWork.Articles
                .Where(ArticleExtensions.IsAvailable(articleId, fromDateTime), new ArticleIncludes { Product = true })
                .SingleOrDefault();

            if (article == null)
            {
                //Article does not exist or is not available.
                return false;
            }


            var orderLine = article.CreateOrderLine(orderId);

            _unitOfWork.OrderLines.Add(orderLine);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        public bool Rent(Guid orderId, IList<Guid> articleIds)
        {
            var fromDateTime = DateTime.Now;
            var articles = _unitOfWork.Articles
                .Where(ArticleExtensions.IsAvailable(articleIds, fromDateTime), new ArticleIncludes { Product = true })
                .ToList();

            //If the amount of articles is not the same as the requested ids, some articles are not available anymore
            if (articleIds.Count != articles.Count)
            {
                return false;
            }

            foreach (var article in articles)
            {
                var orderLine = article.CreateOrderLine(orderId);
                _unitOfWork.OrderLines.Add(orderLine);
            }

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Returns a rented article
        /// </summary>
        /// <param name="orderLineId"></param>
        /// <param name="returnedAt"></param>
        /// <returns></returns>
        public bool Return(Guid orderLineId, DateTime returnedAt)
        {
            var orderLine = _unitOfWork.OrderLines
                .Where(ol => ol.Id == orderLineId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();

            if (orderLine == null)
            {
                return false;
            }

            if (returnedAt == DateTime.MinValue)
            {
                return false;
            }

            if (orderLine.ReturnedAt.HasValue)
            {
                return false;
            }

            orderLine.ReturnedAt = returnedAt;

            _unitOfWork.Complete();
            return true;
        }


    }
}
