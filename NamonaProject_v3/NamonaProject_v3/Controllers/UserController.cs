using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;

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
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                await _userModel.ValidateUser(dto.UserName, dto.Password);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("admin/login")]
        public async Task<ActionResult<UserDto>> AdminLogin(string username, string password)
        {
            try
            {
                var user = _userModel.AdminLogin(username, password);
                if (user == null)
                    return Unauthorized("Invalid admin credentials");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationDto dto)
        {
            try
            {
                await _userModel.Registration(dto.Email, dto.UserName, dto.Password);
                return Ok("User successfully registered");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                return Ok(_userModel.ShowUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userModel.DeleteUser(id);
                return Ok("User deleted");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}/password")]
        public async Task<ActionResult> UpdatePassword(int id, string newPassword)
        {
            try
            {
                await _userModel.UpdatePassword(id, newPassword);
                return Ok("Password updated");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/promote")]
        public async Task<ActionResult> PromoteToAdmin(int id)
        {
            try
            {
               await _userModel.PromoteToAdmin(id);
                return Ok("User promoted to admin");
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}