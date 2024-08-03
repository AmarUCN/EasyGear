using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class DeliveryController : Controller
    {
        DeliveryDAO _deliveryDAO;

        public DeliveryController(DeliveryDAO deliveryDAO)
        {
            _deliveryDAO = deliveryDAO;
        }

        

        
    }
}
