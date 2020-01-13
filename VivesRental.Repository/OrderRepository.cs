using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VivesRental.Model;
using VivesRental.Repository.Contracts;
using VivesRental.Repository.Core;
using VivesRental.Repository.Extensions;
using VivesRental.Repository.Includes;
using VivesRental.Repository.Mappers;
using VivesRental.Repository.Results;

namespace VivesRental.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IVivesRentalDbContext _context;

        public OrderRepository(IVivesRentalDbContext context)
        {
            _context = context;
        }

        public Order Get(Guid id, OrderIncludes includes = null)
        {
            return _context.Orders
                .AddIncludes(includes)
                .SingleOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetAll(OrderIncludes includes = null)
        {
            return _context.Orders
                .AddIncludes(includes)
                .AsEnumerable();
        }

        public IEnumerable<OrderResult> GetAllResult(OrderIncludes includes = null)
        {
            return _context.Orders
                .AddIncludes(includes)
                .MapToResults()
                .AsEnumerable();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public bool ClearCustomer(Guid customerId)
        {
            var commandText = "UPDATE Order SET CustomerId = null WHERE CustomerId = @CustomerId";
            var customerIdParameter = new SqlParameter("@CustomerId", customerId);

            var result = _context.Database.ExecuteSqlRaw(commandText, customerIdParameter);

            return result > 0;
        }
    }
}
