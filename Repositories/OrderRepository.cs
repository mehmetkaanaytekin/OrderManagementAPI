using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.Models;
using static OrderManagementAPI.Dtos.CustomerDTO;
using static OrderManagementAPI.Dtos.OrderDTO;
using static OrderManagementAPI.Dtos.ProductDTO;

namespace OrderManagementAPI.Repositories
{
    public class OrderRepository : IOrderInterface
    {
        private readonly OrdermanagementContext _context;
        private readonly OrderDetailRepository orderDetailRepository;
        private readonly CustomerRepository customerRepository;
        private readonly ProductRepository productRepository;

        public OrderRepository(OrdermanagementContext context,
            OrderDetailRepository orderDetailRepository,
            CustomerRepository customerRepository,
            ProductRepository productRepository)
        {
            _context = context;
            this.orderDetailRepository = orderDetailRepository;
            this.customerRepository = customerRepository;
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
        }

        public async Task<GetOrderDTO> GetOrdersAsDTO(int OrderID)
        {
            var order = await GetOrderAsync(OrderID);
            var orderDetail = await orderDetailRepository.GetOrderDetailAsync(OrderID);
            GetCustomerDTO customer = (customerRepository.GetCustomerAsync(OrderID)).AsDto();

            List<GetProductDTO> productList = new List<GetProductDTO>();

            foreach (var detail in orderDetail)
            {
                productList.Add(productRepository.GetProduct(detail.ProductId).AsDto());
            }

            GetOrderDTO GetOrder = new GetOrderDTO(OrderID, customer, productList);

            return GetOrder;
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
