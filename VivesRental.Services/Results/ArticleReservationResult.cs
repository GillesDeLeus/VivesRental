using System;

namespace VivesRental.Services.Results
{
    public class ArticleReservationResult
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public ArticleResult Article { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerResult Customer { get; set; }

        public DateTime FromDateTime { get; set; }
        public DateTime UntilDateTime { get; set; }
    }
}
