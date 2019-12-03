using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table(nameof(RentalOrderLine))]
    public class RentalOrderLine
    {
        public Guid Id { get; set; }
        public Guid RentalOrderId { get; set; }
        public Order RentalOrder { get; set; }
        public Guid? RentalItemId { get; set; }
        public RentalItem RentalItem { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public DateTime RentedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
