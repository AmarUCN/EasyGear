using DAL.DAO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using WebShop.DTO;

namespace WebShop.Controllers
{
    public class ProductController : Controller
    {
        private ProductDAO _productDAO;

        public ProductController(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }

        public ActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                if (!ModelState.IsValid) { throw new InvalidDataException(); }
                _productDAO.AddProduct(product);
                return RedirectToAction("Details");
            }
            catch { return View(); }
        }

        public IActionResult Details()
        {
            IEnumerable<Product> products = _productDAO.GetAllProducts();
            return View(products);
        }


    }

}
