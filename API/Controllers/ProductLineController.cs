using Microsoft.AspNetCore.Mvc;
using DAL.DAO;
using DAL.Models;

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

        // Create a new ProductLine
        [HttpPost]
        public IActionResult CreateProductLine([FromBody] ProductLine productLine)
        {
            if (productLine == null)
            {
                return BadRequest("ProductLine cannot be null.");
            }

            try
            {
                _productLineDAO.AddProductLine(productLine);
                return CreatedAtAction(nameof(GetProductLineById), new { id = productLine.Id }, productLine);
            }
            catch (Exception ex)
            {
                return StatusCode(201, $"Internal server error: {ex.Message}");
            }
        }

        // Delete a ProductLine
        [HttpDelete("{id}")]
        public IActionResult DeleteProductLine(int id)
        {
            try
            {
                bool result = _productLineDAO.DeleteProductLine(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("ProductLine not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get a ProductLine by ID
        [HttpGet("{id}")]
        public IActionResult GetProductLineById(int id)
        {
            try
            {
                var productLine = _productLineDAO.GetProductLineById(id);
                if (productLine != null)
                {
                    return Ok(productLine);
                }
                return NotFound("ProductLine not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get all ProductLines
        [HttpGet]
        public IActionResult GetAllProductLines()
        {
            try
            {
                var productLines = _productLineDAO.GetAllProductLines();
                return Ok(productLines);
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Update a ProductLine
        [HttpPut("{id}")]
        public IActionResult UpdateProductLine(int id, [FromBody] ProductLine productLine)
        {
            if (productLine == null || productLine.Id != id)
            {
                return BadRequest("ProductLine data is invalid.");
            }

            try
            {
                bool result = _productLineDAO.UpdateProductLine(productLine);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("ProductLine not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }
    }
}



