using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
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
            var query = _context.ArticleReservations
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            query = query.Where(i => i.Id == id); //Add the where clause
            return query.FirstOrDefault();
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
            var query = _context.ArticleReservations
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.Where(predicate).AsEnumerable(); //Add the where clause and return IEnumerable<Article>
        }

        public IEnumerable<ArticleReservation> GetAll(ArticleReservationIncludes includes = null)
        {
            var query = _context.ArticleReservations
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.AsEnumerable();
        }

        private IQueryable<ArticleReservation> AddIncludes(IQueryable<ArticleReservation> query, ArticleReservationIncludes includes)
        {
            if (includes == null)
                return query;

            if (includes.Article)
                query = query.Include(i => i.Article);

            if (includes.Customer)
                query = query.Include(i => i.Customer);

            return query;
        }
    }
}
