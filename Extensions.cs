using OrderManagementAPI.Models;
using static OrderManagementAPI.Dtos.CustomerDTO;
using static OrderManagementAPI.Dtos.OrderDTO;
using static OrderManagementAPI.Dtos.ProductDTO;

namespace OrderManagementAPI
{
    public static class Extensions
    {
        public static GetCustomerDTO AsDto(this Customer customer)
        {
            return new GetCustomerDTO(customer.CustomerId, customer.ContactName, customer.Address);
        }

        public static GetProductDTO AsDto(this Product product)
        {
            return new GetProductDTO(product.ProductId, product.Barcode, product.Description, product.Price, product.OrderDetails.FirstOrDefault(x => x.ProductId == product.ProductId).Quantity);
        }

       //public static GetOrderDTO AsDto(this Order customerOrder)
       //{
       //    return new GetOrderDTO(customerOrder.OrderId, customerOrder.Customer.AsDto(), customerOrder.OrderId);
       //}
    }
}
