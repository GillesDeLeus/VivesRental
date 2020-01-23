﻿using System;

namespace VivesRental.Services.Results
{
    public class CustomerResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int NumberOfOrders { get; set; }
        public int NumberOfPendingOrders { get; set; }
    }
}
