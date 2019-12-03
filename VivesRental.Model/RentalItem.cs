using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table(nameof(RentalItem))]
    public class RentalItem
    {
        public RentalItem()
        {
            RentalOrderLines = new List<RentalOrderLine>();
        }
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public RentalItemStatus Status { get; set; }

        public IList<RentalOrderLine> RentalOrderLines { get; set; }
    }

    public enum RentalItemStatus
    {
        Normal = 0,
        Broken = 1,
        InRepair = 2,
        Lost = 3,
        Destroyed = 4
    }
}
