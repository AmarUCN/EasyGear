using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.Models;
using WebShop.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketDAO _basketDAO;
        private readonly ProductDAO _productDAO;

        public BasketController(BasketDAO basketDAO, ProductDAO productDAO)
        {
            _basketDAO = basketDAO;
            _productDAO = productDAO;
        }

        // GET: Basket/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Basket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DAL.Models.Basket basket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    basket.CreatedAt = DateTime.Now;
                    int basketId = _basketDAO.AddBasket(basket);

                    // Redirect to ProductLine creation for the basket, passing the Basket ID
                    return RedirectToAction("CreateForBasket", "ProductLine", new { basketId = basketId });
                }
                catch (Exception ex)
                {
                    // Log error and show an error view
                    Console.WriteLine($"Error creating basket: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while creating the basket. Please try again.");
                }
            }

            // If the model state is invalid or an error occurs, return to the create view
            return View(basket);
        }

        // GET: Basket/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}














