using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
using System.Security.Authentication;
using System.Security.Claims;

namespace NamonaProject_v3_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserModel _userModel;

        public UserController(UserModel userModel)
        {
            _userModel = userModel;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _userModel.ValidateUser(dto.Email, dto.Password);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("admin/login")]
        public async Task<ActionResult<UserDto>> AdminLogin([FromBody] LoginAdminDTO dto)
        {
            try
            {
                var user = await _userModel.AdminLogin(dto.UserName, dto.Password);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity)
                );

                if (user == null) return Unauthorized(new { message = "Invalid admin credentials" });

                return Ok(user);
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            try
            {
                await _userModel.Register(dto);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ShowUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                return Ok(_userModel.ShowUsers());
            }
            catch 
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userModel.DeleteUser(id);
                return Ok(new { message = "User deleted" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("password")]
        public async Task<ActionResult> UpdatePassword([FromBody]UpdatePasswordDto dto)
        {
            try
            {
                await _userModel.UpdatePassword(dto);
                return Ok();
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("EditUser")]
        public async Task<ActionResult> EditUser([FromBody] UserDto dto)
        {
            try
            {
                await _userModel.EditUser(dto);
                return Ok();
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("promote")]
        public async Task<ActionResult> PromoteToAdmin(PromoteDto dto)
        {
            try
            {
                await _userModel.PromoteToAdmin(dto);
                return Ok();
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch(InvalidCredentialException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }
    }
}