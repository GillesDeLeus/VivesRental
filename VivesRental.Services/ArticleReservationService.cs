using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Core;
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
        public Task<ArticleReservationResult> GetAsync(Guid id)
        {
            return _context.ArticleReservations
                .Where(ar => ar.Id == id)
                .MapToResults()
                .FirstOrDefaultAsync();
        }

        public Task<List<ArticleReservationResult>> AllAsync()
        {
            return _context.ArticleReservations
                .MapToResults()
                .ToListAsync();
        }

        public Task<ArticleReservationResult> CreateAsync(Guid customerId, Guid articleId)
        {
            var articleReservation = new ArticleReservation
            {
                ArticleId = articleId,
                CustomerId = customerId,
                FromDateTime = DateTime.Now,
                UntilDateTime = DateTime.Now.AddMinutes(5)
            };
            return CreateAsync(articleReservation);
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
                return await GetAsync(articleReservation.Id);
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
