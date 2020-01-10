using System;
using System.Linq;
using VivesRental.Model;

namespace VivesRental.Repository.Extensions
{
    public static class ProductExtensions
    {
        public static Func<Product, bool> IsAvailable(DateTime fromDateTime, DateTime? untilDateTime = null)
        {
            return p => p.Articles.All(ArticleExtensions.IsAvailable(fromDateTime, untilDateTime));
        }
    }
}
