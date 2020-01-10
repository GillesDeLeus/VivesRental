using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VivesRental.Model
{
    [Table("ArticleReservation")]
    public class ArticleReservation
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime UntilDateTime { get; set; }
    }
}
