using OrderManagementAPI.Data;
using OrderManagementAPI.Interfaces;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Repositories
{
    public class ProductRepository : IProductInterface
    {
        private readonly OrdermanagementContext _context;

        public ProductRepository(OrdermanagementContext context)
        {
            _context = context;
        }

        public int CheckProductExist(string Barcode)
        {
            int productId;
            if (_context.Products.Any(x => x.Barcode == Barcode))
            {
                productId = _context.Products.First(x => x.Barcode == Barcode).ProductId;
                return productId;
            }

            return 0;
        }

        public async Task<int> CreateProductAsync(Product Product)
        {
            var productID = CheckProductExist(Product.Barcode);

            if (productID != 0)
            {
                return Product.ProductId = productID;
            }
            else
            {
                _context.Products.Add(Product);
                await _context.SaveChangesAsync();

                return Product.ProductId;
            }
        }

        public async Task DeleteProductsync(int ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(int ProductId)
        {
            Product product = _context.Products.FirstOrDefault(x => x.ProductId == ProductId);
            return product;
        }

        public async Task UpdateProductAsync(Product Product)
        {
            throw new NotImplementedException();
        }
    }
}
