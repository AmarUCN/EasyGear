using DAL.Models;

namespace WebShop.Models
{
    public class OrderViewModel
    {
        public Delivery Delivery { get; set; }
        public List<ProductLine> ProductLines { get; set; } // List to hold multiple ProductLines

        public OrderViewModel()
        {
            Delivery = new Delivery();
            ProductLines = new List<ProductLine>(); // Initialize the ProductLines list
        }
    }

}
