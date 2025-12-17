using Microsoft.EntityFrameworkCore;
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
                ClothingName = x.ClothingName,
                Category = x.Category,
                Collection = x.Collection,
                Color = x.Color,
                Price = x.Price,
                Stock = x.Stock,
                GenderId = x.GenderId,
            });
        }
        
        public void DeleteClothes(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void ChangeClothingData(int id, ChangeClothingDataDto dto)
        {
            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingName).First().ClothingId;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Where(x => x.ClothingId == id).First().ClothingName = dto.ClothingName;
                _context.clothes.Where(x => x.ClothingId == id).First().Collection = dto.Collection;
                _context.clothes.Where(x => x.ClothingId == id).First().Category = dto.Category;
                _context.clothes.Where(x => x.ClothingId == id).First().Color = dto.Color;
                _context.clothes.Where(x => x.ClothingId == id).First().Price = dto.Price;
                _context.clothes.Where(x => x.ClothingId == id).First().GenderId = _context.genders.Where(x=> x.GenderType == dto.GenderType).First().GenderId;
                _context.clothes.Where(x => x.ClothingId == id).First().Stock = dto.Stock;

                _context.SaveChanges();
                trx.Commit();
            }
        }
    }
}
