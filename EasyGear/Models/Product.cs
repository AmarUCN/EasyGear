namespace DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }

        public Product(int id, string productName, int price)
        {
            Id = id;
            ProductName = productName;
            Price = price;
        }

        public Product(string productName, int price)
        {
            ProductName = productName;
            Price = price;
        }

        public Product()
        {
        }
    }
}

