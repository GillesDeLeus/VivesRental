using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VivesRental.Model;

namespace VivesRental.Repository.Contracts
{
    public interface IRentalOrderLineRepository
    {
        RentalOrderLine Get(Guid id);
        IEnumerable<RentalOrderLine> Find(Expression<Func<RentalOrderLine, bool>> predicate);
        void Add(RentalOrderLine rentalOrderLine);
    }
}
