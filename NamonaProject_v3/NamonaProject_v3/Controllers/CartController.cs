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

        [HttpGet("/CartContent")]
        public ActionResult<CartDto> GetCartContent()
        {
            try
            {
                return Ok(_cartModel.GetCartContent());
            }
            catch
            {
                return NoContent();
            }
        }
    

        [HttpPut("/EditCart/{id}")]
        public ActionResult EditCart([FromQuery]int id, [FromBody]EditCartDto dto)
        {
            try
            {
                _cartModel.EditCart(id, dto);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("/DeleteCartItem/{id}")]
        public async Task<ActionResult> DeleteCartItem([FromQuery]int id)
        {
            try
            {
                await _cartModel.DeleteClothesFromCart(id);
                 return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
