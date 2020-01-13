using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

        public ArticleReservation Get(Guid id, ArticleReservationIncludes includes = null)
        {
            return _context.ArticleReservations
                .AddIncludes(includes)
                .FirstOrDefault(i => i.Id == id);
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

        public void Add(ArticleReservation article)
        {
            _context.ArticleReservations.Add(article);
        }

        public IEnumerable<ArticleReservation> Find(Expression<Func<ArticleReservation, bool>> predicate, ArticleReservationIncludes includes = null)
        {
            return _context.ArticleReservations
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable(); //Add the where clause and return IEnumerable<Article>
        }

        public IEnumerable<ArticleReservation> GetAll(ArticleReservationIncludes includes = null)
        {
            return _context.ArticleReservations
                .AddIncludes(includes)
                .AsEnumerable();
        }

        
    }
}
