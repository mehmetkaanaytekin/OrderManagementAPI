using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.Dtos
{
    public class CustomerDTO
    {
        public record GetCustomerDTO(int CustomerId, string ContactName, string Address);
        public record CreateCustomerDTO([Required] string ContactName, string Address);
        public record UpdateCustomerDTO([Required] string ContactName, string Address);
    }
}
