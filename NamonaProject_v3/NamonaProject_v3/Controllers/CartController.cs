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
        private readonly UserModel _userModel;

        public CartController(CartModel cartModel, UserModel userModel)
        {
            _cartModel = cartModel;
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

        [Authorize(Roles = "User")]
        [HttpGet("CartContent")]
        public async Task<ActionResult<MyCartDto>> GetCartContent([FromQuery] int userid)
        {
            try
            {
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                return Ok(_cartModel.GetCartContent(currentUserId.Value));
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
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                await _cartModel.EditCart(currentUserId.Value, dto);
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
        [Authorize]
        [HttpPost("addCart")]
        public async Task<ActionResult> AddCart([FromBody] AddToCartDto dto)
        {
            try
            {
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                dto.UserId = currentUserId.Value;
                await _cartModel.AddToCart(dto);
                return StatusCode(StatusCodes.Status201Created);
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
                var currentUserId = await GetAuthenticatedUserId();
                if (!currentUserId.HasValue)
                {
                    return Unauthorized();
                }

                await _cartModel.DeleteClothesFromCart(currentUserId.Value, id);
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
