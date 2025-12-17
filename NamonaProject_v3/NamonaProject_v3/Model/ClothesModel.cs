using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;

namespace NamonaProject_v3_.Model
{
    public class ClothesModel
    {
        private readonly NamonaDbContext _context;
        public ClothesModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AllClothesDto> GetAllClothes()
        {
            return _context.clothes.Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                Category = x.Category,
                Collection = x.Collection,
                Color = x.Color,
                Price = x.Price,
                Stock = x.Stock,
                GenderId = x.GenderId,
            });
        }
    }
}
