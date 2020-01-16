using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository
{
    public class ArticleReservationRepository : IArticleReservationRepository
    {
        private readonly IVivesRentalDbContext _context;
        public ArticleReservationRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public void Remove(Guid id)
        {
            var localEntity = _context.ArticleReservations.Local.SingleOrDefault(e => e.Id == id);
            if (localEntity == null)
            {
                var entity = new ArticleReservation { Id = id };
                _context.ArticleReservations.Attach(entity);
                _context.ArticleReservations.Remove(entity);
            }
            else
            {
                _context.ArticleReservations.Remove(localEntity);
            }
        }

        public void RemoveByArticleId(Guid articleId)
        {
            _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.ArticleId == articleId));
        }

        public void RemoveByProductId(Guid productId)
        {
            _context.ArticleReservations.RemoveRange(_context.ArticleReservations.Where(ar => ar.Article.ProductId == productId));
        }

        public void Add(ArticleReservation article)
        {
            _context.ArticleReservations.Add(article);
        }

        public IEnumerable<ArticleReservation> Where(ArticleReservationIncludes includes = null)
        {
            return _context.ArticleReservations
                .AddIncludes(includes)
                .AsEnumerable();
        }

        public IEnumerable<ArticleReservation> Where(Expression<Func<ArticleReservation, bool>> predicate = null, ArticleReservationIncludes includes = null)
        {
            return _context.ArticleReservations
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable(); 
        }

    }
}
