using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
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
                var user = await _userModel.ValidateUser(dto.UserName, dto.Password);

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
        public ActionResult<UserDto> AdminLogin([FromBody] LoginDto dto)
        {
            try
            {
                var user = _userModel.AdminLogin(dto.UserName, dto.Password);

                if (user == null)
                    return Unauthorized(new { message = "Invalid admin credentials" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            try
            {
                await _userModel.Register(dto);
                return Ok(new { message = "User created successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ShowUsers")]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                return Ok(_userModel.ShowUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userModel.DeleteUser(id);
                return Ok(new { message = "User deleted" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdatePassword(int id, [FromBody] string newPassword)
        {
            try
            {
                await _userModel.UpdatePassword(id, newPassword);
                return Ok(new { message = "Password updated" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/promote")]
        public async Task<ActionResult> PromoteToAdmin(int id)
        {
            try
            {
                await _userModel.PromoteToAdmin(id);
                return Ok(new { message = "User promoted to admin" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _userModel.GetByEmail(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}