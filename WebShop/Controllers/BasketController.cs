using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.Models;
using WebShop.DTO;

namespace WebShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketDAO _basketDao;
        private readonly ProductDAO _productDao;
        private readonly ProductLineDAO _productLineDao;

        public BasketController(BasketDAO basketDao, ProductDAO productDao, ProductLineDAO productLineDao)
        {
            _basketDao = basketDao;
            _productDao = productDao;
            _productLineDao = productLineDao;
        }

        // GET: /Basket/Create
        public IActionResult Create()
        {
            // Prepare view with necessary data
            ViewBag.Products = _productDao.GetAllProducts();
            return View();
        }

        // POST: /Basket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BasketViewModel model)
        {
            try
            {
                var basket = new Basket(model.CreatedAt);
                _basketDao.AddBasket(basket);

                foreach (var productLine in model.ProductLines)
                {
                    var product = _productDao.GetProductById(productLine.ProductId);
                    if (product == null)
                    {
                        ModelState.AddModelError("", $"Product with ID {productLine.ProductId} does not exist.");
                        ViewBag.Products = _productDao.GetAllProducts();
                        return View(model);
                    }

                    var productLineEntry = new ProductLine(productLine.ProductId, productLine.Quantity, basket.Id);

                    // Log before adding product line
                    Console.WriteLine($"Adding ProductLine: ProductId = {productLineEntry.ProductID}, Quantity = {productLineEntry.Quantity}, BasketId = {productLineEntry.BasketID}");

                    _productLineDao.AddProductLine(productLineEntry);
                }

                TempData["Message"] = "Basket and product lines created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating basket and adding product lines: {ex.Message}");
                ViewBag.Products = _productDao.GetAllProducts();
                return View(model);
            }
        }



        // GET: /Basket/Index
        public IActionResult Index()
        {
            var baskets = _basketDao.GetAllBaskets();
            return View(baskets);
        }
    }
}






