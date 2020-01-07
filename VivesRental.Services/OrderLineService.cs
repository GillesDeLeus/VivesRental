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
    public class OrderLineService : IOrderLineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderLineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderLine Get(Guid id)
        {
            return _unitOfWork.OrderLines.Get(id);
        }

        public IList<OrderLine> FindByOrderId(Guid orderId)
        {
            return _unitOfWork.OrderLines.Find(rol => rol.OrderId == orderId).ToList();
        }

        public bool Rent(Guid orderId, Guid articleId)
        {
            var article = _unitOfWork.Articles.Get(articleId, new ArticleIncludes{Product = true});
            var orderLine = article.CreateOrderLine(orderId);

            _unitOfWork.OrderLines.Add(orderLine);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        public bool Rent(Guid orderId, IList<Guid> articleIds)
        {
            var articles = _unitOfWork.Articles.Find(a => articleIds.Contains(a.Id), new ArticleIncludes{Product = true});

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
            var orderLine = _unitOfWork.OrderLines.Get(orderLineId);

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
