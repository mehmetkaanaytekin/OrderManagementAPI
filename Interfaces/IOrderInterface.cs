using OrderManagementAPI.Models;
using static OrderManagementAPI.Dtos.OrderDTO;

namespace OrderManagementAPI.Interfaces
{
    public interface IOrderInterface
    {
        Task<Order> GetOrderAsync(int OrderID);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<int> CreateOrderAsync(Order newOrder);
        Task UpdateOrderAsync(Order updateOrder);
        Task DeleteOrderAsync(Order order);
        bool CheckOrderExist(int orderID);
        Task<GetOrderDTO> GetOrdersAsDTO(int OrderID);
    }
}
