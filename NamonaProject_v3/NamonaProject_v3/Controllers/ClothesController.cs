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
    public class ClothesController : ControllerBase
    {
        private readonly ClothesModel _clothesModel;

        public ClothesController(ClothesModel clothesModel)
        {
            _clothesModel = clothesModel;
        }

        [HttpGet("GetAllClothes")]
        public ActionResult<IEnumerable<AllClothesDto>> GetAllClothes()
        {
            try
            {
                return Ok(_clothesModel.GetAllClothes());
            }

            catch(Exception ex) 
            {
                return BadRequest();
            }
        }
 
        [Authorize(Roles = "Admin")]
        [HttpPost("AddClothes")]

        public async Task<ActionResult> AddClothes([FromBody] AddClothesDto dto)
        {
            try
            {
                await _clothesModel.AddClothes(dto);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("modify")]

        public async Task<ActionResult> ModifyClothes([FromBody] ChangeClothingDataDto dto)
        {
            try
            {
                await _clothesModel.ChangeClothingData(dto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
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
        [HttpDelete("remove")]

        public async Task<ActionResult> DeleteClothes([FromQuery]int id)
        {
            try
            {
                await _clothesModel.DeleteClothes(id);
                return Ok();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
                
            }
        }

        [HttpGet("FilterClothes")]
        public ActionResult<IEnumerable<AllClothesDto>> FilterClothes([FromBody]FilterClothesDto dto)
        {
            try
            {
                return Ok(_clothesModel.FilterClothes2(dto));
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpGet("SearchClothes")]
        public ActionResult<IEnumerable<AllClothesDto>> SearchClothes([FromQuery]string text)
        {
            try
            {
                return Ok(_clothesModel.SearchBar(text));
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
