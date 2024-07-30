using System;
using System.Collections.Generic;
using DAL.Models;

namespace WebShop.DTO
{
    public class OrderConfirmationViewModel
    {
        public int OrderNumber { get; set; }
        public string DeliveredTo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<ProductLineViewModel> ProductLines { get; set; }
    }

    public class ProductLineViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
