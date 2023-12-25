using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class CustomerDAL
    {
        private readonly MyDbContext _context;
        public CustomerDAL()
        {
            _context = new MyDbContext();
        }
        public async Task<CustomerEntityModel> AddCustomer(CustomerEntityModel customer)

        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<CustomerEntityModel>> GetCustomers()
        {
            return await _context.Customer.ToListAsync();
        }

        public async Task<CustomerEntityModel> GetCustomerBYEmail(string customerEmail)
        {
            CustomerEntityModel customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerEmail == customerEmail);
            if (customer == null)
            {
                return null;
            }
            return customer;
        }
    }

}

