using API.DTO;
using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLineController : Controller
    {
        ProductLineDAO _productLineDAO;

        public ProductLineController(ProductLineDAO productLineDAO)
        {
            _productLineDAO = productLineDAO;
        }

        [HttpPost]
        public ActionResult<ProductLine> AddProductLine(ProductLine productLine)
        {
            if (ModelState.IsValid)
            {
                _productLineDAO.AddProductLine(productLine);
                return CreatedAtAction(nameof(GetProductLineById), new {id = productLine.ProductLineID},productLine);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductLine> GetProductLineById(int productLineID) 
        {
            ProductLine productLine = _productLineDAO.GetProductLineById(productLineID);
            if (productLine == null) 
            {
                return NotFound();
            }
            return Ok(productLine);
        }

        // GET: api/Product
        [HttpGet]
        public ActionResult<IEnumerable<ProductLine>> GetAllProductLines()
        {
            var productLines = _productLineDAO.GetAllProductLines();
            return Ok(productLines);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteProductLine(int productLineID)
        {
            bool succes = _productLineDAO.DeleteProductLine(productLineID);
            if (succes) 
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
