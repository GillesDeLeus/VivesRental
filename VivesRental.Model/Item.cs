using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table(nameof(Item))]
    public class Item
    {
        public Item()
        {
            RentalItems = new List<RentalItem>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Publisher { get; set; }
        public int RentalExpiresAfterDays { get; set; }

        public IList<RentalItem> RentalItems { get; set; }
    }
}
