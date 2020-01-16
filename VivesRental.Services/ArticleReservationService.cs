using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public class ArticleReservationService : IArticleReservationService
    {
        private readonly IVivesRentalDbContext _context;

        public ArticleReservationService(IVivesRentalDbContext context)
        {
            _context = context;
        }
        public async Task<ArticleReservationResult> GetAsync(Guid id)
        {
            return await _context.ArticleReservations
                .Where(ar => ar.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }

        public async Task<ArticleReservationResult> GetAsync(Guid id, ArticleReservationIncludes includes)
        {
            return await _context.ArticleReservations
                .AddIncludes(includes)
                .Where(ar => ar.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ArticleReservationResult>> AllAsync()
        {
            return await _context.ArticleReservations
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<List<ArticleReservationResult>> AllAsync(ArticleReservationIncludes includes)
        {
            return await _context.ArticleReservations
                .AddIncludes(includes)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<ArticleReservationResult> CreateAsync(Guid customerId, Guid articleId)
        {
            var articleReservation = new ArticleReservation
            {
                ArticleId = articleId,
                CustomerId = customerId,
                FromDateTime = DateTime.Now,
                UntilDateTime = DateTime.Now.AddMinutes(5)
            };
            return await CreateAsync(articleReservation);
        }

        public async Task<ArticleReservationResult> CreateAsync(ArticleReservation entity)
        {

            if (!entity.IsValid())
            {
                return null;
            }

            var articleReservation = new ArticleReservation
            {
                CustomerId = entity.CustomerId,
                ArticleId = entity.ArticleId,
                FromDateTime = entity.FromDateTime,
                UntilDateTime = entity.UntilDateTime
            };

            _context.ArticleReservations.Add(articleReservation);
            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            if (numberOfObjectsUpdated > 0)
            {
                //Detach and return
                return articleReservation.MapToResult(DateTime.Now, DateTime.MaxValue);
            }
            return null;
        }

        /// <summary>
        /// Removes one ArticleReservation
        /// </summary>
        /// <param name="id">The id of the ArticleReservation</param>
        /// <returns>True if the article reservation was deleted</returns>
        public async Task<bool> RemoveAsync(Guid id)
        {
            _context.ArticleReservations.Remove(id);

            var numberOfObjectsUpdated = await _context.SaveChangesWithConcurrencyIgnoreAsync();
            return numberOfObjectsUpdated > 0;
        }
    }
}
