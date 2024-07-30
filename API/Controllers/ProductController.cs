using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ProductDAO _productDAO;

        public ProductController(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productDAO.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            var products = _productDAO.GetAllProducts();
            return Ok(products);
        }

        // POST: api/Product
        [HttpPost]
        public ActionResult<Product> AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _productDAO.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductID }, product);
        }

        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || product.ProductID != id)
            {
                return BadRequest();
            }
            var existingProduct = _productDAO.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _productDAO.UpdateProduct(product);
            return NoContent();
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var existingProduct = _productDAO.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _productDAO.DeleteProduct(id);
            return NoContent();
        }
    }
}