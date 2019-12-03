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
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Item Get(Guid id)
        {
            return Get(id, null);
        }

        public Item Get(Guid id, ItemIncludes includes)
        {
            return _unitOfWork.Items.Get(id, includes);
        }

        public IList<Item> All()
        {
            return All(null);
        }

        public IList<Item> All(ItemIncludes includes)
        {
            return _unitOfWork.Items.GetAll(includes).ToList();
        }

        public Item Create(Item entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Add a clean item
            var item = new Item
            {
                Name = entity.Name,
                Description = entity.Description,
                Manufacturer = entity.Manufacturer,
                Publisher = entity.Publisher,
                RentalExpiresAfterDays = entity.RentalExpiresAfterDays
            };

            _unitOfWork.Items.Add(item);
            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return item;
            }
            return null;
        }

        public Item Edit(Item entity)
        {
            if (!entity.IsValid())
            {
                return null;
            }

            //Get Item from unitOfWork
            var item = _unitOfWork.Items.Get(entity.Id);
            if (item == null)
            {
                return null;
            }

            //Only update the properties we want to update
            item.Name = entity.Name;
            item.Description = entity.Description;
            item.Manufacturer = entity.Manufacturer;
            item.Publisher = entity.Publisher;
            item.RentalExpiresAfterDays = entity.RentalExpiresAfterDays;

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            if (numberOfObjectsUpdated > 0)
            {
                return entity;
            }
            return null;
        }

        public bool Remove(Guid id)
        {
            var item = _unitOfWork.Items.Get(id, new ItemIncludes { RentalItemsRentalOrderLines = true });
            if (item == null)
                return false;

            //Remove rentalItems
            foreach (var rentalItem in item.RentalItems.ToList())
            {
                //Remove rentalOrderLines
                foreach (var rentalOrderLine in rentalItem.RentalOrderLines.ToList())
                {
                    rentalOrderLine.RentalItem = null;
                    rentalOrderLine.RentalItemId = null;
                }
                _unitOfWork.RentalItems.Remove(rentalItem.Id);
            }
            //Remove item
            _unitOfWork.Items.Remove(item.Id);

            var numberOfObjectsUpdated = _unitOfWork.Complete();
            return numberOfObjectsUpdated > 0;
        }
    }
}
