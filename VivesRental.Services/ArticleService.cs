using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        private readonly IVivesRentalDbContext _context;

        public ArticleService(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public ArticleResult Get(Guid id, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(a => a.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();
        }

        public IList<ArticleResult> All(ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
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
            return _context.Articles
                .AddIncludes(includes)
                .Where(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetAvailableArticles(Guid productId, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(a => a.ProductId == productId &&
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue))
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetRentedArticles(ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(a => a.IsRented())
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToList();
        }

        public IList<ArticleResult> GetRentedArticles(Guid customerId, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(a => a.IsRented(customerId))
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

            _context.Articles.Add(article);

            var numberOfObjectsUpdated = _context.SaveChanges();
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
            var article = _context.Articles
                .Where(a => a.Id == articleId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefault();

            if (article == null)
            {
                return false;
            }

            //Only update the properties we want to update
            article.Status = status;

            var numberOfObjectsUpdated = _context.SaveChanges();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Removes one Article, Removes the ArticleReservations and disconnects OrderLines from the Article
        /// </summary>
        /// <param name="id">The id of the Article</param>
        /// <returns>True if the article was deleted</returns>
        public bool Remove(Guid id)
        {
            var result = _context.RunInTransaction(() =>
            {
                ClearArticleByArticleId(id);
                _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.ArticleId == id));
                _context.Articles.Remove(id);

                var numberOfObjectsUpdated = _context.SaveChangesWithConcurrencyIgnore();
                return numberOfObjectsUpdated > 0;
            });

            return result;
        }

        public bool IsAvailable(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _context.Articles
                .Where(a => a.Id == articleId)
                .All(ArticleExtensions.IsAvailable(articleId, fromDateTime, untilDateTime));
        }

        public bool IsAvailable(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _context.Articles
                .Where(a => articleIds.Contains(a.Id))
                .All(ArticleExtensions.IsAvailable(articleIds, fromDateTime, untilDateTime));
        }

        private void ClearArticleByArticleId(Guid articleId)
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
