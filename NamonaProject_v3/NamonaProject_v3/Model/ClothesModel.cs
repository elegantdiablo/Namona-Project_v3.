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
                Collection = x.Collection,
                Color = x.Color,
                Price = x.Price,
                Stock = x.Stock,
                GenderId = x.GenderId,
            });
        }
        [Authorize(Roles = "Admin")]
        public void ChangeClothingData(int id, ChangeClothingDataDto dto)
        {
            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().ClothingId;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Where(x => x.ClothingId == id).First().ClothingName = dto.ClothingName;
                _context.clothes.Where(x => x.ClothingId == id).First().Collection = dto.Collection;
                _context.clothes.Include(x => x.category).Where(x => x.ClothingId == id).First().category.CategoryName = dto.Category;
                _context.clothes.Where(x => x.ClothingId == id).First().Color = dto.Color;
                _context.clothes.Where(x => x.ClothingId == id).First().Price = dto.Price;
                _context.clothes.Where(x => x.ClothingId == id).First().GenderId = _context.genders.Where(x => x.GenderType == dto.GenderType).First().GenderId;
                _context.clothes.Where(x => x.ClothingId == id).First().Stock = dto.Stock;

                _context.SaveChanges();
                trx.Commit();
            }
        }
        [Authorize(Roles = "Admin")]
        public void DeleteClothes(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
        public IEnumerable<AllClothesDto> FilterClothes(
            string category,
            string collection,
            string gender,
            int minprice = 0,
            int maxprice = 99999999)
        {
            var query = _context.clothes
                .Include(x => x.category)
                .Include(x => x.gender)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(x =>
                    x.category.CategoryName.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(collection))
            {
                query = query.Where(x =>
                    x.Collection.ToLower() == collection.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(x =>
                    x.gender.GenderType.ToLower() == gender.ToLower());
            }

            query = query.Where(x =>
                x.Price >= minprice && x.Price <= maxprice);

            return query.Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                ClothingName = x.ClothingName,
                Collection = x.Collection,
                Category = x.category.CategoryName,
                GenderId = x.GenderId,
                Stock = x.Stock,
                Color = x.Color,
                Price = x.Price
            }).ToList();


        }
        public IEnumerable<AllClothesDto> FilterClothes2(string category, string collection, string gender, int minprice = 0, int maxprice = 99999999)
        {
            var result = _context.clothes
                .AsQueryable();
            if (category != null && collection != null && gender != null)
            {
                result =
                _context.clothes.Include(x => x.gender).Include(x => x.category)
                .Where(x => x.category.CategoryName == category.ToLower() &&
                x.Collection == collection.ToLower() &&
                x.gender.GenderType == gender);
            }
            else if (category == null && collection != null && gender != null)
            {

                result =
               _context.clothes.Include(x => x.gender)
               .Where(x => x.Collection == collection.ToLower() &&
               x.gender.GenderType == gender);
            }
            else if (category != null && collection == null && gender != null)
            {
                _context.clothes.Include(x => x.gender).Include(x => x.category)
                .Where(x => x.category.CategoryName == category.ToLower() &&
                x.gender.GenderType == gender);
            }
            else if (category != null && collection != null && gender == null)
            {
                _context.clothes.Include(x => x.category)
                .Where(x => x.category.CategoryName == category.ToLower() &&
                x.Collection == collection.ToLower() &&
                x.gender.GenderType == gender);
            }

            else if (category == null && collection == null && gender != null)
            {

                _context.clothes.Include(x => x.gender)
                   .Where(x =>
                   x.gender.GenderType == gender);
            }
            else if (category != null && collection == null && gender == null)
            {

                _context.clothes.Include(x => x.category)
                   .Where(x => x.category.CategoryName == category.ToLower()
                 );
            }
            else if (category == null && collection != null && gender == null)
            {

                _context.clothes
                   .Where(x =>
                   x.Collection == collection.ToLower());
            }

            return result.Where(x => x.Price > minprice && x.Price < maxprice).Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                ClothingName = x.ClothingName,
                Collection = x.Collection,
                Category = x.category.CategoryName,
                GenderId = x.GenderId,
                Stock = x.Stock,
                Color = x.Color,
                Price = x.Price
            });
        }
    }
}
