using System.Linq;
using VivesRental.Repository.Core;

namespace VivesRental.Repository.Extensions
{
    public static class DbContextExtensions
    {
        public static bool Exists<TEntity>(this IVivesRentalDbContext context, TEntity entity)
            where TEntity : class
        {
            return context.Set<TEntity>().Local.Any(e => e == entity);
        }
    }
}
