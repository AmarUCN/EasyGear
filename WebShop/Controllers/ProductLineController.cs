using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebShop.Controllers
{
    public class ProductLineController : Controller
    {
        ProductLineDAO _productLineDAO;

        public ProductLineController(ProductLineDAO productLineDAO)
        {
            _productLineDAO = productLineDAO;
        }
        public IActionResult Create()
        {

            return View(new ProductLine());
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductLine productLine)
        {
            try
            {
                if (ModelState.IsValid)

                    _productLineDAO.AddProductLine(productLine);
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }


        }
    }
}
