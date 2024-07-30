namespace DAL.Models
{
    public class Delivery
    {
        public int OrderNumber { get; set; }
        public string DeliveredTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        


        public Delivery(int orderNumber,string deliveredTo, DateTime deliveryDate) : this(deliveredTo,deliveryDate)
        {
            OrderNumber = orderNumber;
            
            
        }

        public Delivery( string deliveredTo, DateTime deliveryDate)
        {
            DeliveredTo = deliveredTo;
            DeliveryDate = deliveryDate;
        }


        public Delivery() { }
    }
}
