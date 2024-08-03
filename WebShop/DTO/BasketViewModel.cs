

namespace WebShop.DTO
{
    public class BasketViewModel
    {
        public DateTime CreatedAt { get; set; }
        public List<ProductLineViewModel> ProductLines { get; set; } = new List<ProductLineViewModel>();
    }
}



