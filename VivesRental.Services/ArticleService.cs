using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ArticleResult> GetAsync(Guid id, ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .Where(a => a.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ArticleResult>> AllAsync(ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<List<ArticleResult>> GetAvailableArticlesAsync(ArticleIncludes includes = null)
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;

            return await GetAvailableArticlesAsync(fromDateTime, untilDateTime, includes);
        }

        public async Task<List<ArticleResult>> GetAvailableArticlesAsync(DateTime fromDateTime, DateTime untilDateTime, ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .Where(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<List<ArticleResult>> GetAvailableArticlesAsync(Guid productId, ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .Where(a => a.ProductId == productId &&
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue))
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<List<ArticleResult>> GetRentedArticlesAsync(ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .Where(a => a.IsRented())
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<List<ArticleResult>> GetRentedArticlesAsync(Guid customerId, ArticleIncludes includes = null)
        {
            return await _context.Articles
                .AddIncludes(includes)
                .Where(a => a.IsRented(customerId))
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<ArticleResult> CreateAsync(Article entity)
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

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
            {
                //Detach and return
                return article.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        public async Task<bool> UpdateStatusAsync(Guid articleId, ArticleStatus status)
        {
            //Get Product from unitOfWork
            var article = await _context.Articles
                .Where(a => a.Id == articleId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();

            if (article == null)
            {
                return false;
            }

            //Only update the properties we want to update
            article.Status = status;

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Removes one Article, Removes the ArticleReservations and disconnects OrderLines from the Article
        /// </summary>
        /// <param name="id">The id of the Article</param>
        /// <returns>True if the article was deleted</returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            var result = await _context.RunInTransactionAsync(async () =>
            {
                await ClearArticleByArticleId(id);
                _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.ArticleId == id));
                _context.Articles.Remove(id);

                var numberOfObjectsUpdated = await _context.SaveChangesWithConcurrencyIgnoreAsync();
                return numberOfObjectsUpdated > 0;
            });

            return await result;
        }

        public async Task<bool> IsAvailableAsync(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return await _context.Articles
                .Where(a => a.Id == articleId)
                .AllAsync(ArticleExtensions.IsAvailable(articleId, fromDateTime, untilDateTime));
        }

        public async Task<bool> IsAvailableAsync(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return await _context.Articles
                .Where(a => articleIds.Contains(a.Id))
                .AllAsync(ArticleExtensions.IsAvailable(articleIds, fromDateTime, untilDateTime));
        }

        private async Task ClearArticleByArticleId(Guid articleId)
        {
            if (_context.Database.IsInMemory())
            {
                var orderLines = await _context.OrderLines.Where(ol => ol.ArticleId == articleId).ToListAsync();
                foreach (var orderLine in orderLines)
                {
                    orderLine.Article = null;
                    orderLine.ArticleId = null;
                }
                return;
            }

            var commandText = "UPDATE OrderLine SET ArticleId = null WHERE ArticleId = @ArticleId";
            var articleIdParameter = new SqlParameter("@ArticleId", articleId);

            await _context.Database.ExecuteSqlRawAsync(commandText, articleIdParameter);
        }

    }
}
