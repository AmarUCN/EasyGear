namespace DAL.Models
{
    public class ProductLine
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public int? BasketID { get; set; } // Nullable to align with your schema
        public int? OrderNumber { get; set; } // Nullable to align with your schema
        public int Quantity { get; set; }

        public ProductLine(int id, int productID, int quantity, int? basketID = null, int? orderNumber = null)
        {
            Id = id;
            ProductID = productID;
            Quantity = quantity;
            BasketID = basketID;
            OrderNumber = orderNumber;
        }

        public ProductLine(int productID, int quantity, int? basketID = null, int? orderNumber = null)
        {
            ProductID = productID;
            Quantity = quantity;
            BasketID = basketID;
            OrderNumber = orderNumber;
        }

        public ProductLine() { }
    }
}



