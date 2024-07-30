namespace DAL.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }

        public Product(int productID, string productName, int price)
        {
            ProductID = productID;
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
