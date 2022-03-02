using System.ComponentModel.DataAnnotations;

namespace OrderManagementAPI.Dtos
{
    public class ProductDTO
    {
        public record GetProductDTO(int ProductId, [Required] string Barcode, string Description, decimal Price, short Quantity);
        public record CreateProductDTO([Required] string Barcode, string Description, decimal Price, short Quantity);
        public record UpdateProductDTO([Required] string Barcode, string Description, decimal Price, short Quantity);
    }
}
