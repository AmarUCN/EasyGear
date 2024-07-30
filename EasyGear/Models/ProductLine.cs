namespace DAL.Models
{
    public class ProductLine
    {
        public int ProductLineID { get; set; }
        public int ProductID { get; set; }
        public int OrderNumber { get; set; }
        public int Quantity { get; set; }

        public ProductLine(int productLineID, int productID, int orderNumber, int quantity)
        {
            ProductLineID = productLineID;
            ProductID = productID;
            OrderNumber = orderNumber;
            Quantity = quantity;
        }

        public ProductLine(int productID, int orderNumber, int quantity)
        {
            ProductID = productID;
            OrderNumber = orderNumber;
            Quantity = quantity;
        }

        public ProductLine() { }

    }
}
