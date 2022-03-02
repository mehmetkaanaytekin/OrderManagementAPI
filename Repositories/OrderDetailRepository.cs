using OrderManagementAPI.Data;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderManagementAPI.Repositories
{
    public class OrderDetailRepository : IOrderDetailInterface
    {
        private readonly OrdermanagementContext _context;

        public OrderDetailRepository(OrdermanagementContext context)
        {
            _context = context;
        }

        public async Task CreateOrderDetailAsync(OrderDetail newOrderDetail)
        {
            _context.OrderDetails.Add(newOrderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(int OrderId)
        {
            _context.OrderDetails.RemoveRange(_context.OrderDetails.Where(x => x.OrderId == OrderId));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductFromOrder(OrderDetail deleteDetail)
        {
            _context.OrderDetails.Remove(deleteDetail);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _context.OrderDetails.ToListAsync();
            return orderDetails;
        }

        public async Task<List<OrderDetail>> GetOrderDetailAsync(int orderId)
        {
            var orderDetails = await _context.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync();
            return orderDetails;
        }

        public async Task UpdateOrderDetailAsync(OrderDetail updateOrderDetail)
        {
            OrderDetail orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderId == updateOrderDetail.OrderId && x.ProductId == updateOrderDetail.ProductId);

            if (orderDetail == null)
            {
                await CreateOrderDetailAsync(updateOrderDetail);
            }
            else if ((orderDetail != null && orderDetail.Quantity != updateOrderDetail.Quantity))
            {
                _context.OrderDetails.Attach(updateOrderDetail);
                _context.Entry(updateOrderDetail).Property(x => x.Quantity).IsModified = true;
                _context.Entry(orderDetail).State = EntityState.Detached;
                await _context.SaveChangesAsync();
            }
            else
            {
                return;
            }
        }
    }
}
