using Microsoft.EntityFrameworkCore;
using NorthTraderAPI.Models;
using NorthTraderAPI.NorthwindServices;

namespace NorthTraderAPI.DataServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly NorthwindDbContext _context;

        public CustomerServices(NorthwindDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.Include(o=>o.Orders).ToListAsync();
        }

        public async Task<Customer?> GetCustomerAsync(string id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
