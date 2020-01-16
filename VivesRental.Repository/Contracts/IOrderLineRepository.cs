using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IOrderLineRepository
    {
        IEnumerable<OrderLine> Where(OrderLineIncludes includes = null);
        IEnumerable<OrderLine> Where(Expression<Func<OrderLine, bool>> predicate, OrderLineIncludes includes = null);
        void Add(OrderLine orderLine);
        void ClearArticleByArticleId(Guid articleId);
        void ClearArticleByProductId(Guid productId);
    }
}
