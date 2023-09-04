using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class OrderDAL
    {
        private readonly MyDbContext _context;
        public OrderDAL()
        {
            _context = new MyDbContext();
        }
        public async Task<Order> AddOrder(Order order)

        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Order.Include(o => o.Customer)
                           .Include(o => o.OrderDetails)
                               .ThenInclude(od => od.Product)
                           .ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            return await _context.Order
                       .Include(o => o.Customer)
                       .Include(o => o.OrderDetails)
                           .ThenInclude(od => od.Product).ThenInclude(od => od.ProductImages)
                       .FirstOrDefaultAsync(o => o.OrderId == id);
        }


    }
}

