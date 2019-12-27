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
    public class ArticleRepository : IArticleRepository
    {
        private readonly IVivesRentalDbContext _context;
        public ArticleRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Article Get(Guid id)
        {
            return Get(id, null);
        }

        public Article Get(Guid id, ArticleIncludes includes)
        {
            var query = _context.Articles.AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            query = query.Where(i => i.Id == id); //Add the where clause
            return query.FirstOrDefault();
        }

        public void Remove(Guid id)
        {
            var entity = new Article { Id = id };
            if (!_context.Exists(entity))
            {
                _context.Articles.Attach(entity);
            }
            _context.Articles.Remove(entity);
        }

        public void Add(Article article)
        {
            _context.Articles.Add(article);
        }

        public IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate)
        {
            return Find(predicate, null);
        }

        public IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate, ArticleIncludes includes)
        {
            var query = _context.Articles
                .AsNoTracking()
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.Where(predicate).AsEnumerable(); //Add the where clause and return IEnumerable<Article>
        }

        public IEnumerable<Article> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Article> GetAll(ArticleIncludes includes)
        {
            var query = _context.Articles
                .AsNoTracking()
                .AsQueryable(); //It needs to be a queryable to be able to build the expression
            query = AddIncludes(query, includes);
            return query.AsEnumerable();
        }

        private IQueryable<Article> AddIncludes(IQueryable<Article> query, ArticleIncludes includes)
        {
            if (includes == null)
                return query;

            if (includes.Product)
                query = query.Include(i => i.Product);

	        if (includes.OrderLines)
		        query = query.Include(i => i.OrderLines);

            return query;
        }
    }
}
