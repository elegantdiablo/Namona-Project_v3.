using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;

namespace NamonaProject_v3_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderModel _orderModel;

        public OrdersController(OrderModel orderModel)
        {
            _orderModel = orderModel;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            try
            {
                return Ok(_orderModel.AllOrders());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddOrder([FromBody] OrderDto order)
        {
            try
            {
                _orderModel.AddOrder(order);
                return Ok("Order successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] OrderDto order)
        {
            try
            {
                _orderModel.UpdateOrder(id, order);
                return Ok("Order successfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _orderModel.DeleteOrder(id);
                return Ok("Order successfully deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/cancel")]
        public IActionResult CancelOrder(int id)
        {
            try
            {
                _orderModel.CancelOrder(id);
                return Ok("Order cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/complete")]
        public IActionResult CompleteOrder(int id)
        {
            try
            {
                _orderModel.CompleteOrder(id);
                return Ok("Order completed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            try
            {
                _orderModel.UpdateOrderStatus(id, status);
                return Ok("Order status updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("clear")]
        public IActionResult ClearOrders()
        {
            try
            {
                _orderModel.ClearOrders();
                return Ok("All orders cleared");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
