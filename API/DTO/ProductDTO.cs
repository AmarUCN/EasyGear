using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class ProductDTO
    {
        [Required]
        public string ProductName { get; set; }

        public ProductDTO(string productName)
        {
            ProductName = productName;
        }

        public ProductDTO()
        {
        }
    }
}
