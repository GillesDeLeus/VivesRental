using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
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

        public Task<ArticleResult> GetAsync(Guid id)
        {
            return _context.Articles
                .Where(a => a.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }

        public Task<List<ArticleResult>> AllAsync()
        {
            return _context.Articles
                .MapToResults()
                .ToListAsync();
        }

        public Task<List<ArticleResult>> GetAvailableArticlesAsync()
        {
            var fromDateTime = DateTime.Now;
            var untilDateTime = DateTime.MaxValue;

            return GetAvailableArticlesAsync(fromDateTime, untilDateTime);
        }

        public Task<List<ArticleResult>> GetAvailableArticlesAsync(DateTime fromDateTime, DateTime untilDateTime)
        {
            return _context.Articles
                .Where(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime))
                .MapToResults()
                .ToListAsync();
        }

        public Task<List<ArticleResult>> GetAvailableArticlesAsync(Guid productId)
        {
            return _context.Articles
                .Where(a => a.ProductId == productId &&
                                                  a.Status == ArticleStatus.Normal &&
                                                  a.OrderLines.All(ol => ol.ReturnedAt.HasValue))
                .MapToResults()
                .ToListAsync();
        }

        public Task<List<ArticleResult>> GetRentedArticlesAsync()
        {
            return _context.Articles
                .Where(a => a.IsRented())
                .MapToResults()
                .ToListAsync();
        }

        public Task<List<ArticleResult>> GetRentedArticlesAsync(Guid customerId)
        {
            return _context.Articles
                .Where(a => a.IsRented(customerId))
                .MapToResults()
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
                return await GetAsync(article.Id);
            }
            return null;
        }

        public async Task<bool> UpdateStatusAsync(Guid articleId, ArticleStatus status)
        {
            //Get Product from unitOfWork
            var article = await _context.Articles
                .Where(a => a.Id == articleId)
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
            if (_context.Database.IsInMemory())
            {
                await ClearArticleByArticleId(id);
                _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.ArticleId == id));
                _context.Articles.Remove(id);
                return true;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await ClearArticleByArticleId(id);
                _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.ArticleId == id));
                _context.Articles.Remove(id);

                var numberOfObjectsUpdated = await _context.SaveChangesWithConcurrencyIgnoreAsync();
                await transaction.CommitAsync();
                return numberOfObjectsUpdated > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<bool> IsAvailableAsync(Guid articleId, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _context.Articles
                .Where(a => a.Id == articleId)
                .AllAsync(ArticleExtensions.IsAvailable(articleId, fromDateTime, untilDateTime));
        }

        public Task<bool> IsAvailableAsync(IList<Guid> articleIds, DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return _context.Articles
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
