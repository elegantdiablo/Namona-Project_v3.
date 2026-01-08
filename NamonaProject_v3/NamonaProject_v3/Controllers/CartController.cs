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
        public ActionResult EditCart(int id, CartItemDto dto)
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
        public ActionResult DeleteCartItem(int id)
        {
            try
            {
                _cartModel.DeleteClothes(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
