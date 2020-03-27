using System;
using VivesRental.Model;

namespace VivesRental.Services.Results
{
    public class ArticleResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public ArticleStatus Status { get; set; }
    }
}
