using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly DeliveryDAO _deliveryDAO;

        public DeliveryController(DeliveryDAO deliveryDAO)
        {
            _deliveryDAO = deliveryDAO;
        }

        // GET: Delivery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Delivery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DAL.Models.Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int deliveryId = _deliveryDAO.AddOrder(delivery);

                    // Redirect to ProductLine creation, passing the Delivery ID
                    return RedirectToAction("Create", "ProductLine", new { deliveryId = deliveryId });
                }
                catch (Exception ex)
                {
                    // Log error and show an error view
                    Console.WriteLine($"Error adding delivery: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while adding the delivery. Please try again.");
                }
            }

            // If the model state is invalid or an error occurs, return to the create view
            return View(delivery);
        }
    }
}




