namespace DAL.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string DeliveredTo { get; set; }
        public DateTime DeliveryDate { get; set; }

        public Delivery(int id, string deliveredTo, DateTime deliveryDate)
        {
            Id = id;
            DeliveredTo = deliveredTo;
            DeliveryDate = deliveryDate;
        }

        public Delivery(string deliveredTo, DateTime deliveryDate)
        {
            DeliveredTo = deliveredTo;
            DeliveryDate = deliveryDate;
        }

        public Delivery() { }
    }
}


