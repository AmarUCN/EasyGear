using System.Collections.Generic;
using DAL.Models;

namespace WebShop.DTO
{
    public class BasketViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<ProductLine> Basket { get; set; } = new List<ProductLine>();
        public int SelectedProductId { get; set; }
        public int Quantity { get; set; }
        public int OrderNumber { get; set; }
    }
}
