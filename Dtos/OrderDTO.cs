using System.ComponentModel.DataAnnotations;
using static OrderManagementAPI.Dtos.CustomerDTO;
using static OrderManagementAPI.Dtos.ProductDTO;

namespace OrderManagementAPI.Dtos
{
    public class OrderDTO
    {
        public record GetOrderDTO(int OrderID, GetCustomerDTO Customer, List<GetProductDTO> Products);
        public record CreateOrderDTO(CreateCustomerDTO Customer, CreateProductDTO[] Products);
        public record UpdateOrderDTO(int OrderID, UpdateCustomerDTO Customer, UpdateProductDTO[] Products);
    }
}