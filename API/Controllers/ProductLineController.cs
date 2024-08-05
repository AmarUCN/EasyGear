using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLineController : ControllerBase
    {
        private readonly ProductLineDAO _productLineDAO;

        public ProductLineController(ProductLineDAO productLineDAO)
        {
            _productLineDAO = productLineDAO;
        }

        // GET: api/ProductLine/{id}
        [HttpGet("{id}")]
        public ActionResult<ProductLine> GetProductLineById(int id)
        {
            var productLine = _productLineDAO.GetProductLineById(id);
            if (productLine == null)
            {
                return NotFound();
            }
            return Ok(productLine);
        }

        // GET: api/ProductLine
        [HttpGet]
        public ActionResult<IEnumerable<ProductLine>> GetAllProductLines()
        {
            var productLines = _productLineDAO.GetAllProductLines();
            return Ok(productLines);
        }

        // POST: api/ProductLine
        [HttpPost]
        public ActionResult<ProductLine> AddProductLine(ProductLine productLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add the ProductLine to the database
            _productLineDAO.AddProductLine(productLine);
            return CreatedAtAction(nameof(GetProductLineById), new { id = productLine.Id }, productLine);
        }

        // PUT: api/ProductLine/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateProductLine(int id, [FromBody] ProductLine productLine)
        {
            if (productLine == null || productLine.Id != id)
            {
                return BadRequest();
            }
            var existingProductLine = _productLineDAO.GetProductLineById(id);
            if (existingProductLine == null)
            {
                return NotFound();
            }

            _productLineDAO.UpdateProductLine(productLine);
            return NoContent();
        }

        // DELETE: api/ProductLine/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteProductLine(int id)
        {
            var existingProductLine = _productLineDAO.GetProductLineById(id);
            if (existingProductLine == null)
            {
                return NotFound();
            }

            _productLineDAO.DeleteProductLine(id);
            return NoContent();
        }
    }
}


