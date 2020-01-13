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
        
        public Article Get(Guid id, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .FirstOrDefault(i => i.Id == id);
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

        public void Add(Article article)
        {
            _context.Articles.Add(article);
        }

        public IEnumerable<Article> Find(Expression<Func<Article, bool>> predicate, ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .Where(predicate)
                .AsEnumerable(); //Add the where clause and return IEnumerable<Article>
        }

        public IEnumerable<Article> GetAll(ArticleIncludes includes = null)
        {
            return _context.Articles
                .AddIncludes(includes)
                .AsEnumerable();
        }
    }
}
