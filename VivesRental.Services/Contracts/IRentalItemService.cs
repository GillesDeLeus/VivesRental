using System;
using System.Collections.Generic;
using VivesRental.Model;

namespace VivesRental.Services.Contracts
{
    public interface IRentalItemService
    {
        RentalItem Get(Guid id);
	    IList<RentalItem> All();
	    IList<RentalItem> GetAvailableRentalItems();
	    IList<RentalItem> GetRentedRentalItems();
	    IList<RentalItem> GetRentedRentalItems(Guid customerId);

		RentalItem Create(RentalItem entity);
        RentalItem Edit(RentalItem entity);
        bool Remove(Guid id);
    }
}
