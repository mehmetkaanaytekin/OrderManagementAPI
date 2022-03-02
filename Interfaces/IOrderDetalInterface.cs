using OrderManagementAPI.Models;

namespace OrderManagementAPI.Interfaces
{
    public interface IOrderDetailInterface
    {
        Task<OrderDetail> GetOrderDetailAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync();
        Task CreateOrderDetailAsync(OrderDetail newOrderDetail);
        Task UpdateOrderDetailAsync(OrderDetail updateOrderDetail);
        Task DeleteOrderDetailAsync(int OrderId);
    }
}
