using System;
using VivesRental.Model;

namespace VivesRental.Services.Results
{
    public class ArticleReservationResult
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public ArticleStatus ArticleStatus { get; set; }
        public string ProductName { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime UntilDateTime { get; set; }
    }
}
