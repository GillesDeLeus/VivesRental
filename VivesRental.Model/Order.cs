using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table(nameof(Order))]
    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }
        [Key]
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public IList<OrderLine> OrderLines { get; set; }
    }
}
