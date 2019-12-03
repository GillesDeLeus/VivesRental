using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Core;
using VivesRental.Repository.Includes;
using VivesRental.Services.Contracts;
using VivesRental.Services.Extensions;

namespace VivesRental.Services
{
    public class RentalItemService : IRentalItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public RentalItem Get(Guid id)
        {
            return _unitOfWork.RentalItems.Get(id);
        }

        public RentalItem Get(Guid id, RentalItemIncludes includes)
        {
            return _unitOfWork.RentalItems.Get(id, includes);
        }

        public IList<RentalItem> All()
        {
            return _unitOfWork.RentalItems.GetAll().ToList();
        }

        public IList<RentalItem> All(RentalItemIncludes includes)
        {
            return _unitOfWork.RentalItems.GetAll(includes).ToList();
        }

        public IList<RentalItem> GetAvailableRentalItems()
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.All(rol => rol.ReturnedAt.HasValue)).ToList();
        }

        public IList<RentalItem> GetAvailableRentalItems(RentalItemIncludes includes)
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.All(rol => rol.ReturnedAt.HasValue), includes).ToList();
        }

        public IList<RentalItem> GetRentedRentalItems()
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.Any(rol => !rol.ReturnedAt.HasValue)).ToList();
        }

        public IList<RentalItem> GetRentedRentalItems(RentalItemIncludes includes)
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.Any(rol => !rol.ReturnedAt.HasValue), includes).ToList();
        }

        public IList<RentalItem> GetRentedRentalItems(Guid customerId)
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.RentalOrder.CustomerId == customerId)).ToList();
        }

        public IList<RentalItem> GetRentedRentalItems(Guid customerId, RentalItemIncludes includes)
        {
            return _unitOfWork.RentalItems.Find(ri => ri.Status == RentalItemStatus.Normal &&
                                                         ri.RentalOrderLines.Any(rol => !rol.ReturnedAt.HasValue && rol.RentalOrder.CustomerId == customerId), includes).ToList();
        }

        public RentalItem Create(RentalItem entity)
        {

            if (!entity.IsValid())
            {
                return null;
            }

            var rentalItem = new RentalItem
            {
                ItemId = entity.ItemId,
                Status = entity.Status
            };

            _unitOfWork.RentalItems.Add(rentalItem);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return rentalItem;
            }
            return null;
        }

        public RentalItem Edit(RentalItem entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Item from unitOfWork
            var rentalItem = _unitOfWork.RentalItems.Get(entity.Id);
            if (rentalItem == null)
            {
                return null;
            }

            //Only update the properties we want to update
            rentalItem.ItemId = entity.ItemId;
            rentalItem.Status = entity.Status;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return entity;
            }
            return null;
        }

        public bool Remove(Guid id)
        {
            var rentalItem = _unitOfWork.RentalItems.Get(id, new RentalItemIncludes { RentalOrderLines = true });
            if (rentalItem == null)
                return false;

            foreach (var rentalOrderLine in rentalItem.RentalOrderLines)
            {
                rentalOrderLine.RentalItem = null;
                rentalOrderLine.RentalItemId = null;
            }

            _unitOfWork.RentalItems.Remove(rentalItem.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
