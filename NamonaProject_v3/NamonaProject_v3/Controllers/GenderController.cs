using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

namespace NamonaProject_v3_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly GenderModel _model;

        public GenderController(GenderModel model)
        {
            _model = model;
        }

        [HttpGet("AllGenders")]
        public ActionResult<NamonaDbContext> GetAllGenders()
        {
            try
            {
                return Ok(_model.GetAllGenders());
            }

            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddGender")]

        public async Task<ActionResult> AddGender(AddGenderDto dto)
        {
            try
            {
                await _model.AddGender(dto);
                return Ok();    
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("ModifyGender")]
        public async Task<ActionResult> ModifyGender(EditGenderDto dto)
        {
            try
            {
                await _model.EditGender(dto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteGender")]
        public async Task<ActionResult> DeleteGender(int id)
        {
            try
            {
                await _model.DeleteGender(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }
    }
}
