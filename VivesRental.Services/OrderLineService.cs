using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class OrderLineService : IOrderLineService
    {
        private readonly IVivesRentalDbContext _context;

        public OrderLineService(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Task<OrderLineResult> GetAsync(Guid id)
        {
            return _context.OrderLines
                .Where(ol => ol.Id == id)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }

        public Task<List<OrderLineResult>> FindByOrderIdAsync(Guid orderId)
        {
            return _context.OrderLines
                .Where(rol => rol.OrderId == orderId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .ToListAsync();
        }

        public async Task<bool> RentAsync(Guid orderId, Guid articleId)
        {
            var fromDateTime = DateTime.Now;

            var article = await _context.Articles
                .Include(a => a.Product)
                .Where(ArticleExtensions.IsAvailable(articleId, fromDateTime))
                .SingleOrDefaultAsync();

            if (article == null)
            {
                //Article does not exist or is not available.
                return false;
            }


            var orderLine = article.CreateOrderLine(orderId);

            _context.OrderLines.Add(orderLine);
            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            return numberOfObjectsUpdated > 0;
        }

        public async Task<bool> RentAsync(Guid orderId, IList<Guid> articleIds)
        {
            var fromDateTime = DateTime.Now;
            //DateTime? untilDateTime = null;
            var articles = await _context.Articles
                .Include(a => a.Product) //Needs include for the CreateOrderLine extension method.
                .Where(ArticleExtensions.IsAvailable(articleIds, fromDateTime))
                .ToListAsync();

            //If the amount of articles is not the same as the requested ids, some articles are not available anymore
            if (articleIds.Count != articles.Count)
            {
                return false;
            }

            foreach (var article in articles)
            {
                var orderLine = article.CreateOrderLine(orderId);
                _context.OrderLines.Add(orderLine);
            }

            var numberOfObjectsUpdated = await _context.SaveChangesAsync();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Returns a rented article
        /// </summary>
        /// <param name="orderLineId"></param>
        /// <param name="returnedAt"></param>
        /// <returns></returns>
        public async Task<bool> ReturnAsync(Guid orderLineId, DateTime returnedAt)
        {
            var orderLine = await _context.OrderLines
                .Where(ol => ol.Id == orderLineId)
                .MapToResults(DateTime.Now, DateTime.MaxValue)
                .FirstOrDefaultAsync();

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

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
