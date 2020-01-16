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
    public class ArticleRepository : IArticleRepository
    {
        private readonly IVivesRentalDbContext _context;
        public ArticleRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public void Remove(Guid id)
        {
            var localEntity = _context.Articles.Local.SingleOrDefault(e => e.Id == id);
            if (localEntity == null)
            {
                var entity = new Article { Id = id };
                _context.Articles.Attach(entity);
                _context.Articles.Remove(entity);
            }
            else
            {
                _context.Articles.Remove(localEntity);
            }
        }

        public void RemoveByProductId(Guid productId)
        {
            _context.Articles.RemoveRange(_context.Articles.Where(a => a.ProductId == productId));
        }

        public void Add(Article article)
        {
            _context.Articles.Add(article);
        }

        public IEnumerable<Article> Where(ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .AsEnumerable();
        }

        public IEnumerable<Article> Where(Expression<Func<Article, bool>> predicate, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable();
        }

        public bool All(Guid id, Expression<Func<Article, bool>> predicate)
        {
            return _context.Articles
                .Where(a => a.Id == id)
                .All(predicate);
        }

        public bool All(IList<Guid> ids, Expression<Func<Article, bool>> predicate)
        {
            return _context.Articles
                .Where(a => ids.Contains(a.Id))
                .All(predicate);
        }
    }
}
