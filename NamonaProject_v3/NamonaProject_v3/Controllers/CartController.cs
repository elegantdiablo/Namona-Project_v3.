using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;

namespace NamonaProject_v3_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartModel _cartModel;

        public CartController(CartModel cartModel)
        {
            _cartModel = cartModel;
        }

        [Authorize(Roles = "User")]
        [HttpGet("CartContent")]
        public ActionResult<CartDto> GetCartContent([FromQuery]int userid)
        {
            try
            {
                return Ok(_cartModel.GetCartContent(userid));
            }
            catch
            {
                return NoContent();
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("EditCart")]
        public async Task<ActionResult> EditCart([FromBody]EditCartDto dto)
        {
            try
            {
                await _cartModel.EditCart(dto);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            catch(InvalidDataException)
            {
                return StatusCode(406);
            }
            catch(Exception ex) 
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost("addCart")]
        public async Task<ActionResult> AddCart([FromBody] AddToCartDto dto)
        {
            try
            {
                await _cartModel.AddToCart(dto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidDataException)
            {
                return StatusCode(406);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("DeleteCartItem")]
        public async Task<ActionResult> DeleteCartItem([FromQuery]int id)
        {
            try
            {
                await _cartModel.DeleteClothesFromCart(id);
                 return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
    }
}
