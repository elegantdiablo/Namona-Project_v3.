using Microsoft.AspNetCore.Authorization;
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
        //[Authorize(Roles = "Admin")]
        public void ChangeClothingData(int id, ChangeClothingDataDto dto)
        {
            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().ClothingId;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Where(x => x.ClothingId == id).First().ClothingName = dto.ClothingName;
                _context.clothes.Where(x => x.ClothingId == id).First().Collection = dto.Collection;
                _context.clothes.Where(x => x.ClothingId == id).First().Category = dto.Category;
                _context.clothes.Where(x => x.ClothingId == id).First().Color = dto.Color;
                _context.clothes.Where(x => x.ClothingId == id).First().Price = dto.Price;
                _context.clothes.Where(x => x.ClothingId == id).First().GenderId = _context.genders.Where(x => x.GenderType == dto.GenderType).First().GenderId;
                _context.clothes.Where(x => x.ClothingId == id).First().Stock = dto.Stock;

                _context.SaveChanges();
                trx.Commit();
            }
        }
        //[Authorize(Roles = "Admin")]
        public void DeleteClothes(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
       /* public IEnumerable<AllClothesDto> FilterClothes(string category, string collection, string gender, int minprice = 0, int maxprice= 99999999)
        {
            if(category != null && collection != null && gender != null)
            {
                //where all
            }
            if(category == null && collection != null && gender != null)
            {

            }
            if (category != null && collection == null && gender != null)
            {

            }
            if (category != null && collection != null && gender == null)
            {

            }
            if (category == null && collection == null && gender != null)
            {

            }
            if (category != null && collection == null && gender == null)
            {

            }
            if (category == null && collection != null && gender == null)
            {

            }
        }
       */
    }
}
