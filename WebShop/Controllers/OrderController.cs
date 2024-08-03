using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly DeliveryDAO _deliveryDAO;
        private readonly ProductLineDAO _productLineDAO;

        public OrderController(DeliveryDAO deliveryDAO, ProductLineDAO productLineDAO)
        {
            _deliveryDAO = deliveryDAO;
            _productLineDAO = productLineDAO;
        }

        
    }
}
