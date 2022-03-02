using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Repositories
{
    public class OrderRepository : IOrderInterface
    {
        private readonly OrdermanagementContext _context;

        public OrderRepository(OrdermanagementContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderAsync(Order newOrder)
        {
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return newOrder.OrderId;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderAsync(int orderID)
        {
            var order = await _context.Orders.FindAsync(orderID);

            return order;
        }

        public bool CheckOrderExist(int orderID)
        {
            return _context.Orders.Any(e => e.OrderId == orderID);
        }

        public async Task UpdateOrderAsync(Order updateOrder)
        {
            var order = _context.Orders.AsNoTracking().FirstOrDefault(e => e.OrderId == updateOrder.OrderId);
            if (order == null) return;
            _context.Entry(order).CurrentValues.SetValues(updateOrder);
            await _context.SaveChangesAsync();



        }


    }
}
