using OrderManagementAPI.Models;

namespace OrderManagementAPI.Interfaces
{
    public interface ICustomerInterface
    {
        Task<Customer> GetCustomerAsync(int CustomerId);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<int> CreateCustomerAsync(Customer newCustomer);
        Task UpdateCustomerAsync(Customer updateCustomer);
        Task DeleteCustomerAsync(int CustomerId);
        int CheckCustomerExist(string customerName);
    }
}
