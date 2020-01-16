using System; 
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository
{
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly IVivesRentalDbContext _context;

        public OrderLineRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderLine> Find(OrderLineIncludes includes = null)
        {
            return _context.OrderLines
                .AddIncludes(includes)
                .AsEnumerable();
        }

        public IEnumerable<OrderLine> Find(Expression<Func<OrderLine, bool>> predicate, OrderLineIncludes includes = null)
        {
            return _context.OrderLines
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public void Add(OrderLine orderLine)
        {
            _context.OrderLines.Add(orderLine);
        }

        public void ClearArticleByProductId(Guid productId)
        {
            if (_context.Database.IsInMemory())
            {
                var orderLines = _context.OrderLines.Where(ol => ol.Article.ProductId == productId).ToList();
                foreach (var orderLine in orderLines)
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }
                return;
            }

            var commandText = "UPDATE OrderLine SET ArticleId = null from OrderLine inner join Article on Article.ProductId = @ProductId";
            var articleIdParameter = new SqlParameter("@ProductId", productId);

            _context.Database.ExecuteSqlRaw(commandText, articleIdParameter);
        }

        public void ClearArticleByArticleId(Guid articleId)
        {
            if (_context.Database.IsInMemory())
            {
                var orderLines = _context.OrderLines.Where(ol => ol.ArticleId == articleId).ToList();
                foreach (var orderLine in orderLines)
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }
                return;
            }

            var commandText = "UPDATE OrderLine SET ArticleId = null WHERE ArticleId = @ArticleId";
            var articleIdParameter = new SqlParameter("@ArticleId", articleId);

            _context.Database.ExecuteSqlRaw(commandText, articleIdParameter);
        }
    }
}
