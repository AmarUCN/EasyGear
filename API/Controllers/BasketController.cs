using Microsoft.AspNetCore.Mvc;
using DAL.DAO;
using DAL.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketDAO _basketDAO;

        public BasketController(BasketDAO basketDAO)
        {
            _basketDAO = basketDAO;
        }

        // Create a new basket
        [HttpPost]
        public IActionResult CreateBasket([FromBody] Basket basket)
        {
            if (basket == null)
            {
                return BadRequest("Basket cannot be null.");
            }

            try
            {
                _basketDAO.AddBasket(basket);
                return CreatedAtAction(nameof(GetBasketById), new { id = basket.Id }, basket);
            }
            catch (Exception ex)
            {
                return StatusCode(201, $"Internal server error: {ex.Message}");
            }
        }

        // Delete a basket
        [HttpDelete("{id}")]
        public IActionResult DeleteBasket(int id)
        {
            try
            {
                bool result = _basketDAO.RemoveBasket(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("Basket not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get a basket by ID
        [HttpGet("{id}")]
        public IActionResult GetBasketById(int id)
        {
            try
            {
                var basket = _basketDAO.GetBasketById(id);
                if (basket != null)
                {
                    return Ok(basket);
                }
                return NotFound("Basket not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get all baskets
        [HttpGet]
        public IActionResult GetAllBaskets()
        {
            try
            {
                var baskets = _basketDAO.GetAllBaskets();
                return Ok(baskets);
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }
    }
}




