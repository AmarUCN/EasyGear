using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace WebShop.Controllers
{
    public class ProductLineController : Controller
    {
        private readonly ProductDAO _productDAO;
        private readonly ProductLineDAO _productLineDAO;

        public ProductLineController(ProductDAO productDAO, ProductLineDAO productLineDAO)
        {
            _productDAO = productDAO;
            _productLineDAO = productLineDAO;
        }

        // GET: ProductLine/Create
        [HttpGet]
        public IActionResult Create(int deliveryId)
        {
            var products = _productDAO.GetAllProducts();
            ViewBag.Products = products;

            // Prepare a list of ProductLine entries
            var productLines = new List<ProductLine>
            {
                new ProductLine() // Add an initial empty ProductLine entry
            };

            ViewData["DeliveryId"] = deliveryId;
            return View(productLines);
        }

        // POST: ProductLine/Create
        [HttpPost]
        public IActionResult Create(List<ProductLine> productLines)
        {
            var deliveryId = productLines.FirstOrDefault()?.OrderNumber ?? 0; // Extract Delivery ID from the first item in the list

            if (ModelState.IsValid)
            {
                foreach (var productLine in productLines)
                {
                    // Ensure that the OrderNumber is set
                    productLine.OrderNumber = deliveryId;

                    try
                    {
                        // Add each ProductLine to the database
                        _productLineDAO.AddProductLine(productLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding product line: {ex.Message}");
                        ModelState.AddModelError("", "An error occurred while adding some product lines. Please try again.");
                        ViewBag.Products = _productDAO.GetAllProducts(); // Re-fetch products in case of error
                        ViewData["DeliveryId"] = deliveryId; // Pass delivery ID back to the view
                        return View(productLines);
                    }
                }

                // Redirect to a confirmation page or another view
                return RedirectToAction("Index");
            }

            // If model state is invalid, re-fetch products for the view
            ViewBag.Products = _productDAO.GetAllProducts();
            ViewData["DeliveryId"] = deliveryId; // Pass delivery ID back to the view
            return View(productLines);
        }

        [HttpGet]
        public IActionResult Created()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateForBasket(int basketId)
        {
            var products = _productDAO.GetAllProducts();
            ViewBag.Products = products;

            // Prepare a list of ProductLine entries
            var productLines = new List<ProductLine>
            {
                new ProductLine() // Add an initial empty ProductLine entry
            };

            ViewData["BasketId"] = basketId;
            return View(productLines);
        }

        // POST: ProductLine/CreateForBasket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateForBasket(List<ProductLine> productLines)
        {
            var basketId = productLines.FirstOrDefault()?.BasketID ?? 0; // Extract Basket ID from the first item in the list

            if (ModelState.IsValid)
            {
                foreach (var productLine in productLines)
                {
                    // Ensure that the BasketID is set
                    productLine.BasketID = basketId;

                    try
                    {
                        // Add each ProductLine to the database
                        _productLineDAO.AddProductLine(productLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding product line: {ex.Message}");
                        ModelState.AddModelError("", "An error occurred while adding some product lines. Please try again.");
                        ViewBag.Products = _productDAO.GetAllProducts(); // Re-fetch products in case of error
                        ViewData["BasketId"] = basketId; // Pass basket ID back to the view
                        return View(productLines);
                    }
                }

                // Redirect to a confirmation page or another view
                return RedirectToAction("Index");
            }

            // If model state is invalid, re-fetch products for the view
            ViewBag.Products = _productDAO.GetAllProducts();
            ViewData["BasketId"] = basketId; // Pass basket ID back to the view
            return View(productLines);
        }

        [HttpGet]
        public IActionResult CreatedForBasket()
        {
            return View();
        }

    }
}










