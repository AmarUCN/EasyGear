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

        public IActionResult Create()
        {

            return View(new Delivery());
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Delivery delivery)
        {
            try
            {
                if (ModelState.IsValid)

                    _deliveryDAO.AddOrder(delivery);
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }


        }
    }
}
