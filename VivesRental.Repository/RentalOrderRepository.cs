using System;
using System.Collections.Generic;
using System.Linq;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;

namespace VivesRental.Repository
{
    public class RentalOrderRepository : IRentalOrderRepository
    {
        private readonly IVivesRentalDbContext _context;

        public RentalOrderRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Order Get(Guid id)
        {
            return _context.RentalOrders.Find(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.RentalOrders.AsEnumerable();
        }

        public void Add(Order rentalOrder)
        {
            _context.RentalOrders.Add(rentalOrder);
        }
    }
}
