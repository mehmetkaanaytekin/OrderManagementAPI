﻿#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.Models;
using OrderManagementAPI.Repositories;
using static OrderManagementAPI.Dtos.CustomerDTO;
using static OrderManagementAPI.Dtos.OrderDTO;
using static OrderManagementAPI.Dtos.ProductDTO;

namespace OrderManagementAPI.Controller
{
    [Route("Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository orderRepository;
        private readonly CustomerRepository customerRepository;
        private readonly ProductRepository productRepository;
        private readonly OrderDetailRepository orderDetailRepository;
        private readonly ILogger<OrdersController> logger;
        private readonly OrdermanagementContext _context;

        public OrdersController
            (ILogger<OrdersController> logger,
            OrderRepository orderRepository,
            OrdermanagementContext context,
            CustomerRepository customerRepository,
            ProductRepository productRepository,
            OrderDetailRepository orderDetailRepository)
        {
            this.orderRepository = orderRepository;
            this.customerRepository = customerRepository;
            this.productRepository = productRepository;
            this.orderDetailRepository = orderDetailRepository;
            this.logger = logger;
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("id")]
        public async Task<ActionResult<GetOrderDTO>> GetOrder(int OrderID)
        {
            var order = (await orderRepository.GetOrderAsync(OrderID));

            if (order == null)
            {
                return NotFound();
            }

            var orderDetail = await orderDetailRepository.GetOrderDetailAsync(OrderID);
            var customer = (await customerRepository.GetCustomerAsync(OrderID)).AsDto;
            var products = (await productRepository.GetProductAsync(OrderID)).AsDto;


            return Ok(order);
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("id")]
        public async Task<IActionResult> PutOrder(int orderID, string customerName, UpdateOrderDTO existingOrder)
        {
            if (orderID != existingOrder.OrderID || customerName != existingOrder.Customer.ContactName)
            {
                return BadRequest();
            }

            Customer updateCustomer = new Customer()
            {
                ContactName = _context.Customers.FirstOrDefault(x => x.ContactName == customerName).ContactName,
                Address = existingOrder.Customer.Address,
            };

            await customerRepository.UpdateCustomerAsync(updateCustomer);

            Order updateOrder = new Order()
            {
                OrderId = orderID,
                OrderDate = _context.Orders.FirstOrDefault(x => x.OrderId == orderID).OrderDate,
                CustomerId = _context.Customers.FirstOrDefault(x => x.ContactName == customerName).CustomerId,
                UpdateDate = DateTime.Now
            };

            await orderRepository.UpdateOrderAsync(updateOrder);

            foreach (UpdateProductDTO product in existingOrder.Products)
            {

                Product newProduct = new Product()
                {
                    Barcode = product.Barcode,
                    Price = product.Price,
                    Description = product.Description,
                };

                await productRepository.CreateProductAsync(newProduct);

                OrderDetail updateOrderDetail = new OrderDetail()
                {
                    OrderId = updateOrder.OrderId,
                    Price = newProduct.Price,
                    ProductId = newProduct.ProductId,
                    Quantity = product.Quantity
                };

                await orderDetailRepository.UpdateOrderDetailAsync(updateOrderDetail);
            }

            return Ok();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CreateOrderDTO>> PostOrder(CreateOrderDTO customerOrder)
        {
            Customer newCustomer = new Customer()
            {
                Address = customerOrder.Customer.Address,
                ContactName = customerOrder.Customer.ContactName,
            };

            await customerRepository.CreateCustomerAsync(newCustomer);

            Order newOrder = new Order()
            {
                CustomerId = newCustomer.CustomerId,
                OrderDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };

            await orderRepository.CreateOrderAsync(newOrder);

            foreach (CreateProductDTO product in customerOrder.Products)
            {

                Product newProduct = new Product()
                {
                    Barcode = product.Barcode,
                    Price = product.Price,
                    Description = product.Description,
                };

                await productRepository.CreateProductAsync(newProduct);

                OrderDetail newOrderDetail = new OrderDetail()
                {
                    OrderId = newOrder.OrderId,
                    Price = newProduct.Price,
                    ProductId = newProduct.ProductId,
                    Quantity = product.Quantity
                };

                await orderDetailRepository.CreateOrderDetailAsync(newOrderDetail);
            }

            return CreatedAtAction("GetOrder", new { id = newOrder.OrderId }, newOrder);
        }

        // DELETE: api/Orders/5
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteOrder(int OrderID)
        {
            var order = await orderRepository.GetOrderAsync(OrderID);
            if (order == null)
            {
                return NotFound();
            }

            await orderDetailRepository.DeleteOrderDetailAsync(order.OrderId);
            await orderRepository.DeleteOrderAsync(order);

            return NoContent();
        }

        [HttpDelete("Barcode")]
        public async Task<IActionResult> DeleteProductFromOrder(int OrderID, string Barcode)
        {
            int productID = productRepository.CheckProductExist(Barcode);
            if (productID == 0)
            {
                return NotFound();
            }

            var orderDetail = _context.OrderDetails.FirstOrDefault(x => x.OrderId == OrderID && x.ProductId == productID);
            await orderDetailRepository.DeleteProductFromOrder(orderDetail);

            return NoContent();
        }
    }
}