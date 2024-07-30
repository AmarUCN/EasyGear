using API.DTO;
using DAL.DAO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : Controller
    {
        DeliveryDAO _deliveryDAO;

        public DeliveryController(DeliveryDAO deliveryDAO) 
        {
            _deliveryDAO = deliveryDAO;
        }
        [HttpPost]
        public ActionResult<Delivery> Post(Delivery delivery)
        {
            if (ModelState.IsValid)
            {
                _deliveryDAO.AddOrder(delivery);
                return CreatedAtAction(nameof(GetOrderById), new { id = delivery.OrderNumber }, delivery);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public ActionResult<Delivery> GetOrderById(int orderNumber)
        {
            Delivery delivery = _deliveryDAO.GetOrderById(orderNumber);
            if (delivery == null)
            {
                return NotFound();
            }
            return Ok(delivery);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int orderNumber)
        {
            bool succes = _deliveryDAO.DeleteOrder(orderNumber);
            if (succes)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Delivery>> GetAllOrders()
        {
            var deliveryes = _deliveryDAO.GetAllOrders();

            if (deliveryes == null)
            {
                return NotFound();
            }

            return Ok(deliveryes);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int orderNumber, [FromBody] Delivery delivery)
        {
            if (delivery == null || delivery.OrderNumber != orderNumber)
            {
                return BadRequest();
            }
            var existingDelivery = _deliveryDAO.GetOrderById(orderNumber);
            if (existingDelivery == null)
            {
                return NotFound();
            }
            _deliveryDAO.UpdateOrder(delivery);
            return NoContent();
        }

    }
}
