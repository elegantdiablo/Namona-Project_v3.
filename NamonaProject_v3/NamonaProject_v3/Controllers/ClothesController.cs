using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/AllClothes")]
        public ActionResult<NamonaDbContext> GetAllClothes()
        {
            try
            {
                return Ok(_clothesModel.GetAllClothes());
            }

            catch
            {
                return NoContent();
            }

        }

    }
}
