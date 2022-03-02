using OrderManagementAPI.Models;

namespace OrderManagementAPI.Interfaces
{
    public interface IProductInterface
    {
        Product GetProduct(int ProductId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<int> CreateProductAsync(Product newProduct);
        Task UpdateProductAsync(Product newProduct);
        Task DeleteProductsync(int ProductId);
        int CheckProductExist(string Barcode);
    }
}
