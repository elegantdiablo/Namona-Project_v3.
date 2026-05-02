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
        private readonly UserModel _userModel;

        public OrdersController(OrderModel orderModel, UserModel userModel)
        {
            _orderModel = orderModel;
            _userModel = userModel;
        }

        private async Task<int?> GetAuthenticatedUserId()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return null;
            }

            var email = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var user = await _userModel.GetByEmail(email);
            return user?.UserId;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("AllOrders")]
        public ActionResult<IEnumerable<OrderDto>> GetAllOrders()
        {
            try
            {
                return Ok(_orderModel.AllOrders());
            }
            catch 
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderById")]
        public ActionResult<OrderDto> GetOrderById(int id)
        {
            try
            {
                return Ok(_orderModel.GetOrderById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderHistoryDto>>> GetOrders([FromQuery]int userid)
        {
            try
            {
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                return Ok(_orderModel.GetOrdersForUser(currentUserId.Value));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] CheckoutOrderDto dto)
        {
            try
            {
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                var orderId = await _orderModel.CheckoutOrder(currentUserId.Value, dto);
                return Ok(new { orderId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidDataException)
            {
                return StatusCode(406);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddOrder")]
        public async Task<ActionResult> AddOrder([FromBody] AddOrderDto order)
        {
            try
            {
                await _orderModel.AddOrder(order);
                return StatusCode(201, "Order successfully added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrder")]
        public async Task<ActionResult> UpdateOrder([FromBody] OrderDto order)
        {
            try
            {
                await _orderModel.UpdateOrder(order);
                return Ok("Order successfully updated");
            }
            catch(InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteOrder")]
        public async Task<ActionResult> DeleteOrder([FromQuery] int id)
        {
            try
            {
                await _orderModel.DeleteOrder(id);
                return Ok("Order successfully deleted");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch 
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("cancel")]
        public async Task<ActionResult> CancelOrder([FromQuery]int id)
        {
            try
            {
                await _orderModel.DeleteOrder(id);
                return Ok("Order cancelled");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Complete")]
        public async Task<ActionResult> CompleteOrder([FromQuery]int id)
        {
            try
            {
               await _orderModel.CompleteOrder(id);
                return Ok("Order completed");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("status")]
        public async Task<ActionResult> UpdateOrderStatus([FromBody]UpdateStatusDto dto)
        {
            try
            {
                await _orderModel.UpdateOrderStatus(dto);
                return Ok("Order status updated");
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetRevenue")]
        public async Task<ActionResult<int>> GetRevenue()
        {
            try
            {
                return Ok(_orderModel.Revenue());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
