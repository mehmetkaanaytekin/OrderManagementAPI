using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Repositories
{
    public class CustomerRepository : ICustomerInterface
    {
        private readonly OrdermanagementContext _context;

        public CustomerRepository(OrdermanagementContext context)
        {
            _context = context;
        }

        public int CheckCustomerExist(string customerName)
        {
            int customerId;
            if (_context.Customers.Any(x => x.ContactName == customerName))
            {
                customerId = _context.Customers.First(x => x.ContactName == customerName).CustomerId;
                return customerId;
            }

            return 0;
        }

        public async Task<int> CreateCustomerAsync(Customer newCustomer)
        {
            var customerId = CheckCustomerExist(newCustomer.ContactName);

            if (customerId != 0)
            {
                return newCustomer.CustomerId = customerId;
            }
            else
            {
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();

                return newCustomer.CustomerId;
            }
        }

        public Task DeleteCustomerAsync(int CustomerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomerAsync(int CustomerId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCustomerAsync(Customer updateCustomer)
        {
            
            if (CheckCustomerExist(updateCustomer.ContactName) != 0)
            {
                if (_context.Customers.Any(x => x.ContactName == updateCustomer.ContactName && x.Address == updateCustomer.Address)) return;

                _context.Customers.Update(updateCustomer);
                _context.Entry(updateCustomer).Property(x => x.Address).IsModified = true;
                await _context.SaveChangesAsync();
                _context.Entry(updateCustomer).State = EntityState.Detached;

            }            
        }
    }
}
