using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IRentalOrderLineService
    {
        RentalOrderLine Get(Guid id);
        bool Rent(Guid rentalOrderId, Guid rentalItemId);
        bool Return(Guid rentalOrderLineId, DateTime returnedAt);
	    IList<RentalOrderLine> FindByRentalOrderId(Guid rentalOrderId);

    }
}
