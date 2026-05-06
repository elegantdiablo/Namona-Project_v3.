using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;

namespace NamonaProject_v3_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryModel _model;

        public CategoryController(CategoryModel model)
        {
            _model = model;
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<AllCategoryDto>>> GetAllCategories()
        {
            try
            {
                return Ok(_model.AllCategories());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategory([FromBody]AddCategoryDto dto)
        {
            try
            {
                await _model.AddCategory(dto);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("EditCategory")]
        public async Task<ActionResult> EditCategory([FromBody]EditCategoryDto dto)
        {
            try
            {
                await _model.EditCategory(dto);
                return Ok();
            }
            catch (InvalidDataException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch (InvalidOperationException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory")]
        public async Task<ActionResult> DeleteCategory([FromQuery]int id)
        {
            try
            {
                await _model.DeleteCategory(id);
                return Ok();
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
    }
}
