using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Services.Contracts;

namespace VivesRental.Services
{
    public class RentalOrderLineService : IRentalOrderLineService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalOrderLineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public RentalOrderLine Get(Guid id)
        {
            return _unitOfWork.RentalOrderLines.Get(id);
        }

        public IList<RentalOrderLine> FindByRentalOrderId(Guid rentalOrderId)
        {
            return _unitOfWork.RentalOrderLines.Find(rol => rol.RentalOrderId == rentalOrderId).ToList();
        }

        public bool Rent(Guid rentalOrderId, Guid rentalItemId)
        {
            var rentalItem = _unitOfWork.RentalItems.Get(rentalItemId);
            var item = _unitOfWork.Items.Get(rentalItem.ItemId);
            var rentalOrderLine = new RentalOrderLine
            {
                RentalItemId = rentalItemId,
                RentalOrderId = rentalOrderId,
                ItemName = item.Name,
                ItemDescription = item.Description,
                ExpiresAt = DateTime.Now.AddDays(item.RentalExpiresAfterDays),
                RentedAt = DateTime.Now
            };

            _unitOfWork.RentalOrderLines.Add(rentalOrderLine);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }

        /// <summary>
        /// Returns a rented item
        /// </summary>
        /// <param name="rentalOrderLineId"></param>
        /// <param name="returnedAt"></param>
        /// <returns></returns>
        public bool Return(Guid rentalOrderLineId, DateTime returnedAt)
        {
            var rentalOrderLine = _unitOfWork.RentalOrderLines.Get(rentalOrderLineId);

            if (rentalOrderLine == null)
            {
                return false;
            }

            if (returnedAt == DateTime.MinValue)
            {
                return false;
            }

            if (rentalOrderLine.ReturnedAt.HasValue)
            {
                return false;
            }

            rentalOrderLine.ReturnedAt = returnedAt;

            _unitOfWork.Complete();
            return true;
        }
    }
}
