using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;
using VivesRental.Repository.Includes;

namespace VivesRental.Repository.Contracts
{
    public interface IRentalItemRepository
    {
        IEnumerable<RentalItem> GetAll();
        IEnumerable<RentalItem> GetAll(RentalItemIncludes includes);

        IEnumerable<RentalItem> Find(Expression<Func<RentalItem, bool>> predicate);
        IEnumerable<RentalItem> Find(Expression<Func<RentalItem, bool>> predicate, RentalItemIncludes includes);

        RentalItem Get(Guid id);
        RentalItem Get(Guid id, RentalItemIncludes includes);

        void Remove(Guid id);
        void Add(RentalItem rentalItem);
        
    }
}
