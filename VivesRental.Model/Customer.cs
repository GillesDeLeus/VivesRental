using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VivesRental.Model.Contracts;

namespace VivesRental.Model
{
    [Table(nameof(Customer))]
    public class Customer: IIdentifiable
    {
        public Customer()
        {
            Orders = new List<Order>();
            ArticleReservations = new List<ArticleReservation>();
        }

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IList<Order> Orders { get; set; }
        public IList<ArticleReservation> ArticleReservations { get; set; }
    }
}
