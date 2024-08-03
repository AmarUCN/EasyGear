namespace DAL.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string DeliveredTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int AccountID { get; set; }

        public Delivery(int id, string deliveredTo, DateTime deliveryDate, int accountID)
        {
            Id = id;
            DeliveredTo = deliveredTo;
            DeliveryDate = deliveryDate;
            AccountID = accountID;
        }

        public Delivery(string deliveredTo, DateTime deliveryDate, int accountID)
        {
            DeliveredTo = deliveredTo;
            DeliveryDate = deliveryDate;
            AccountID = accountID;
        }

        public Delivery() { }
    }
}


