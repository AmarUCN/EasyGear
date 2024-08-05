using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.DAO;
using DAL.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DeliveryDAO _deliveryDAO;

        public DeliveryController(DeliveryDAO deliveryDAO)
        {
            _deliveryDAO = deliveryDAO;
        }

        // Create a new delivery
        [HttpPost]
        public IActionResult CreateOrder([FromBody] DAL.Models.Delivery delivery)
        {
            if (delivery == null)
            {
                return BadRequest("Delivery cannot be null.");
            }

            try
            {
                // Get the ID of the newly created delivery
                int newDeliveryId = _deliveryDAO.AddOrder(delivery);

                if (newDeliveryId <= 0)
                {
                    return StatusCode(500, "An error occurred while creating the delivery.");
                }

                // Set the ID on the delivery object
                delivery.Id = newDeliveryId;

                return CreatedAtAction(nameof(GetOrderById), new { id = delivery.Id }, delivery);
            }
            catch (Exception ex)
            {
                return StatusCode(201, $"Internal server error: {ex.Message}");
            }
        }

        // Delete a delivery
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                bool result = _deliveryDAO.DeleteOrder(id);
                if (result)
                {
                    return NoContent();
                }
                return NotFound("Delivery not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get a delivery by ID
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var delivery = _deliveryDAO.GetOrderById(id);
                if (delivery != null)
                {
                    return Ok(delivery);
                }
                return NotFound("Delivery not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Get all deliveries
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                var deliveries = _deliveryDAO.GetAllOrders();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }

        // Update a delivery
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Delivery delivery)
        {
            if (delivery == null || delivery.Id != id)
            {
                return BadRequest("Delivery ID mismatch.");
            }

            try
            {
                var existingDelivery = _deliveryDAO.GetOrderById(id);
                if (existingDelivery == null)
                {
                    return NotFound("Delivery not found.");
                }

                bool result = _deliveryDAO.UpdateOrder(delivery);
                if (result)
                {
                    return NoContent();
                }

                return StatusCode(200, "Update failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"Internal server error: {ex.Message}");
            }
        }
    }
}



