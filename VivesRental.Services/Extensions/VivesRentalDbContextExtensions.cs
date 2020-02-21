using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VivesRental.Model.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Services.Extensions
{
    public static class VivesRentalDbContextExtensions
    {
        //public static T RunInTransaction<T>(this IVivesRentalDbContext context, Func<T> logic)
        //{
        //    if (context.Database.IsInMemory())
        //    {
        //        return logic();
        //    }

        //    using var transaction = context.Database.BeginTransaction();
        //    try
        //    {
        //        var result = logic();
        //        transaction.Commit();
        //        return result;
        //    }
        //    catch
        //    {
        //        transaction.Rollback();
        //        throw;
        //    }
        //}

        //public static async Task<T> RunInTransactionAsync<T>(this IVivesRentalDbContext context, Func<T> logic)
        //{
        //    if (context.Database.IsInMemory())
        //    {
        //        return logic();
        //    }

        //    await using var transaction = await context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var result = logic();
        //        await transaction.CommitAsync();
        //        return result;
        //    }
        //    catch
        //    {
        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}

        public static EntityEntry<T> Remove<T>(this DbSet<T> dbSet, Guid id)
            where T : class, IIdentifiable
        {
            var entity = dbSet.Local.SingleOrDefault(e => e.Id == id);
            if (entity != null)
            {
                return dbSet.Remove(entity);
            }

            entity = Activator.CreateInstance<T>();
            entity.Id = id;
            dbSet.Attach(entity);

            return dbSet.Remove(entity);
        }

        /// <summary>
        /// Needed for concurrency exception when id does not exist in the database
        /// https://stackoverflow.com/questions/19295232/how-to-ignore-a-dbupdateconcurrencyexception-when-deleting-an-entity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int SaveChangesWithConcurrencyIgnore(this IVivesRentalDbContext context)
        {
            while (true)
            {
                try
                {
                    return context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();

                    if (entry.State == EntityState.Deleted)
                        //When EF deletes an item its state is set to Detached
                        //http://msdn.microsoft.com/en-us/data/jj592676.aspx
                        entry.State = EntityState.Detached;
                    else
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            }
        }

        /// <summary>
        /// Needed for concurrency exception when id does not exist in the database
        /// https://stackoverflow.com/questions/19295232/how-to-ignore-a-dbupdateconcurrencyexception-when-deleting-an-entity
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<int> SaveChangesWithConcurrencyIgnoreAsync(this IVivesRentalDbContext context)
        {
            while (true)
            {
                try
                {
                    return await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();

                    if (entry.State == EntityState.Deleted)
                        //When EF deletes an item its state is set to Detached
                        //http://msdn.microsoft.com/en-us/data/jj592676.aspx
                        entry.State = EntityState.Detached;
                    else
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }
            }
        }
    }
}
