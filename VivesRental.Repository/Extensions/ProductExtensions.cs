using System;
using System.Linq;
using System.Linq.Expressions;
using VivesRental.Model;

namespace VivesRental.Repository.Extensions
{
    public static class ProductExtensions
    {
        public static Expression<Func<Product, bool>> IsAvailable(DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return p => p.Articles.AsQueryable().Any(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime));
        }

    }
}
