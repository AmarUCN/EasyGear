using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebShop.DTO;

namespace WebShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly ProductDAO _productDAO;
        private readonly ProductLineDAO _productLineDAO;
        private readonly DeliveryDAO _deliveryDAO;
        private static Basket _basket = new Basket();
        private static int? _currentOrderNumber;

        public BasketController(ProductDAO productDAO, ProductLineDAO productLineDAO, DeliveryDAO deliveryDAO)
        {
            _productDAO = productDAO;
            _productLineDAO = productLineDAO;
            _deliveryDAO = deliveryDAO;
        }

        public IActionResult Index()
        {
            var viewModel = new BasketViewModel
            {
                Products = _productDAO.GetAllProducts().ToList(),
                Basket = _basket.ProductLines.Values.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToBasket(BasketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // If there's no current order, create a new delivery
                if (_currentOrderNumber == null)
                {
                    var delivery = new Delivery
                    {
                        DeliveredTo = "Pending", // Placeholder until checkout
                        DeliveryDate = DateTime.Now // Current date/time
                    };
                    _deliveryDAO.AddOrder(delivery);
                    _currentOrderNumber = delivery.OrderNumber;
                }

                var productLine = new ProductLine
                {
                    ProductID = model.SelectedProductId,
                    OrderNumber = _currentOrderNumber.Value,
                    Quantity = model.Quantity
                };

                _basket.AddOrUpdateProduct(productLine);
            }

            model.Products = _productDAO.GetAllProducts().ToList();
            model.Basket = _basket.ProductLines.Values.ToList();
            return View("Index", model);
        }

        [HttpPost]
        public IActionResult UpdateProductQuantity(int productID, int quantity)
        {
            _basket.UpdateProductQuantity(productID, quantity);
            var updatedProductLine = _basket.ProductLines[productID];
            _productLineDAO.UpdateProductLine(updatedProductLine); // Update in database

            var viewModel = new BasketViewModel
            {
                Products = _productDAO.GetAllProducts().ToList(),
                Basket = _basket.ProductLines.Values.ToList()
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult RemoveFromBasket(int productID)
        {
            _basket.RemoveProduct(productID);
            _productLineDAO.DeleteProductLine(productID); // Remove from database

            var viewModel = new BasketViewModel
            {
                Products = _productDAO.GetAllProducts().ToList(),
                Basket = _basket.ProductLines.Values.ToList()
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult ClearBasket()
        {
            _basket.Clear();
            _currentOrderNumber = null; // Clear the current order number
            var viewModel = new BasketViewModel
            {
                Products = _productDAO.GetAllProducts().ToList(),
                Basket = new List<ProductLine>()
            };

            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Checkout(string deliveredTo, DateTime deliveryDate)
        {
            if (_basket.ProductLines.Any())
            {
                // Create a new delivery entry
                var delivery = new Delivery
                {
                    DeliveredTo = deliveredTo,
                    DeliveryDate = deliveryDate
                };

                try
                {
                    // Insert the delivery and get the new OrderNumber
                    _deliveryDAO.AddOrder(delivery);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    ModelState.AddModelError("", $"Failed to create order: {ex.Message}");
                    var errorViewModel = new BasketViewModel
                    {
                        Products = _productDAO.GetAllProducts().ToList(),
                        Basket = _basket.ProductLines.Values.ToList()
                    };
                    return Json(new { success = false, message = "Failed to create order." });
                }

                int orderNumber = delivery.OrderNumber;

                // Check if OrderNumber is correctly set
                if (orderNumber <= 0)
                {
                    // Handle error
                    ModelState.AddModelError("", "Failed to create order.");
                    var errorViewModel = new BasketViewModel
                    {
                        Products = _productDAO.GetAllProducts().ToList(),
                        Basket = _basket.ProductLines.Values.ToList()
                    };
                    return Json(new { success = false, message = "Failed to create order." });
                }

                // Associate the delivery with the product lines in the basket
                var productLines = new List<ProductLineViewModel>();
                foreach (var productLine in _basket.ProductLines.Values)
                {
                    productLine.OrderNumber = orderNumber;
                    _productLineDAO.AddProductLine(productLine);

                    // Get product details for the confirmation view
                    var product = _productDAO.GetProductById(productLine.ProductID);
                    if (product != null)
                    {
                        productLines.Add(new ProductLineViewModel
                        {
                            ProductName = product.ProductName,
                            Quantity = productLine.Quantity,
                            Price = product.Price
                        });
                    }
                }

                // Clear the basket after checkout
                _basket.Clear();

                // Prepare the order confirmation view model
                var orderConfirmationViewModel = new OrderConfirmationViewModel
                {
                    OrderNumber = orderNumber,
                    DeliveredTo = deliveredTo,
                    DeliveryDate = deliveryDate,
                    ProductLines = productLines
                };

                return View("OrderConfirmation", orderConfirmationViewModel);
            }

            // If the basket is empty, redirect to the index with an error message
            ModelState.AddModelError("", "Your basket is empty.");
            var viewModel = new BasketViewModel
            {
                Products = _productDAO.GetAllProducts().ToList(),
                Basket = _basket.ProductLines.Values.ToList()
            };

            return Json(new { success = false, message = "Your basket is empty." });
        }


    }
}
