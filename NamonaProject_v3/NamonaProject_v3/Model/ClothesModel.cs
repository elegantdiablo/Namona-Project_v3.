using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Namona_v3.DTO;
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
            return _context.clothes.Include(x => x.Category).Include(x => x.Gender).Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                ClothingName = x.ClothingName,
                Collection = x.Collection,
                Size = x.Size,
                Color = x.Color,
                Price = x.Price,
                Stock = x.Stock,
                GenderName = x.Gender.GenderType,
                CategoryName   = x.Category.CategoryName
            });
        }

        
        public async Task AddClothes(AddClothesDto dto)
        {
            if (!_context.categories.Any(x => x.CategoryName == dto.CategoryName))
            {
                throw new KeyNotFoundException();
            }
            if (!_context.genders.Any(x => x.GenderType == dto.GenderName))
            {
                throw new KeyNotFoundException();
            }
            if(dto.ClothingName == null || dto.Collection == null || dto.Color == null || dto.Price <= 0 || dto.Stock < 0 || dto.Size == null)
            {
                throw new InvalidDataException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Add(new Clothes
                {
                    ClothingName = dto.ClothingName,
                    Collection = dto.Collection,
                    CategoryId = dto.CategoryId,
                    GenderId = dto.GenderId,
                    Size = dto.Size,
                    Stock = dto.Stock,
                    Color = dto.Color,
                    Price = dto.Price,

                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();

            }
            await Task.CompletedTask;
        }

        public async Task ChangeClothingData(ChangeClothingDataDto dto)
        {
            
            if (!_context.categories.Any(x => x.CategoryName == dto.Category))
            {
                throw new KeyNotFoundException();
            }
            if (!_context.genders.Any(x => x.GenderType == dto.GenderType))
            {
                throw new KeyNotFoundException();
            }
            if(!_context.clothes.Any(x => x.ClothingId == dto.ClothingId))
            {
                throw new KeyNotFoundException();
            }
            if (dto.ClothingName == null || dto.Collection == null || dto.Color == null || dto.Price <= 0 || dto.Stock < 0 || dto.Size == null)
            {
                throw new InvalidDataException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().ClothingName = dto.ClothingName;
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Collection = dto.Collection;
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Color = dto.Color;
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Price = dto.Price;
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Size = dto.Size;
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().GenderId = dto.GenderId; //???
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().CategoryId = dto.CategoryId; //???
                _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Stock = dto.Stock;

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }
        public async Task DeleteClothes(int id)
        {
            if(!_context.clothes.Any(x => x.ClothingId == id))
            {
                throw new KeyNotFoundException();   
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }



        /*
        public IEnumerable<AllClothesDto> FilterClothes(    
            string category,
            string collection,
            string gender,
            int minprice = 0,
            int maxprice = 99999999)
        {
            var query = _context.clothes
                .Include(x => x.Category)
                .Include(x => x.Gender)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(x =>
                    x.Category.CategoryName.ToLower() == category.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(collection))
            {
                query = query.Where(x =>
                    x.Collection.ToLower() == collection.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(gender))
            {
                query = query.Where(x =>
                    x.Gender.GenderType.ToLower() == gender.ToLower());
            }

            query = query.Where(x =>
                x.Price >= minprice && x.Price <= maxprice);

            return query.Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                ClothingName = x.ClothingName,
                Collection = x.Collection,
                Category = x.Category.CategoryName,
                GenderId = x.GenderId,
                Stock = x.Stock,
                Color = x.Color,
                Price = x.Price
            }).ToList();

        
        }*/
        public IEnumerable<AllClothesDto> FilterClothes2(FilterClothesDto dto)
        {
            var result = _context.clothes
                .AsQueryable();
            if(dto.Minprice == 0)
            {

            dto.Minprice = _context.clothes.Min(x => x.Price);
            }if (dto.Maxprice == 999999)
            {
                dto.Maxprice = _context.clothes.Max(x => x.Price);
            }
            if (dto.Category != null && dto.Collection != null && dto.Gender != null)
            {
                result =
                _context.clothes.Include(x => x.Gender).Include(x => x.Category)
                .Where(x => x.Category.CategoryName == dto.Category.ToLower() &&
                x.Collection == dto.Collection.ToLower() &&
                x.Gender.GenderType == dto.Gender);
            }
            else if (dto.Category == null && dto.Collection != null && dto.Gender != null)
            {

                result =
               _context.clothes.Include(x => x.Gender)
               .Where(x => x.Collection == dto.Collection.ToLower() &&
               x.Gender.GenderType == dto.Gender);
            }
            else if (dto.Category != null && dto.Collection == null && dto.Gender != null)
            {
                result = _context.clothes.Include(x => x.Gender).Include(x => x.Category)
                .Where(x => x.Category.CategoryName == dto.Category.ToLower() &&
                x.Gender.GenderType == dto.Gender);
            }
            else if (dto.Category != null && dto.Collection != null && dto.Gender == null)
            {
                result = _context.clothes.Include(x => x.Category)
                .Where(x => x.Category.CategoryName == dto.Category.ToLower() &&
                x.Collection == dto.Collection.ToLower() &&
                x.Gender.GenderType == dto.Gender);
            }

            else if (dto.Category == null && dto.Collection == null && dto.Gender != null)
            {

                result = _context.clothes.Include(x => x.Gender)
                   .Where(x =>
                   x.Gender.GenderType == dto.Gender);
            }
            else if (dto.Category != null && dto.Collection == null && dto.Gender == null)
            {

                result = _context.clothes.Include(x => x.Category)
                   .Where(x => x.Category.CategoryName == dto.Category.ToLower()
                 );
            }
            else if (dto.Category == null && dto.Collection != null && dto.Gender == null)
            {

                result = _context.clothes
                   .Where(x =>
                   x.Collection == dto.Collection.ToLower());
            }

            return result.Include(x=> x.Category).Include(x=> x.Gender)
                .Where(x => x.Price > dto.Minprice && x.Price < dto.Maxprice)
                .Select(x => new AllClothesDto
            {
                ClothingId = x.ClothingId,
                ClothingName = x.ClothingName,
                Collection = x.Collection,
                CategoryName = x.Category.CategoryName,
                GenderName = x.Gender.GenderType,
                Stock = x.Stock,
                Color = x.Color,
                Price = x.Price
            });
        }

        public IEnumerable<AllClothesDto> SearchBar(string text)
        {
            return _context.clothes
                .Include(x => x.Category).Include(x => x.Gender)
                .Where(x => x.Category.CategoryName.ToLower().Contains(text.ToLower()) ||
                x.Gender.GenderType.ToLower().Contains(text.ToLower()) ||
                x.Collection.ToLower().Contains(text.ToLower()))
                .Select(x => new AllClothesDto
                {
                    ClothingId = x.ClothingId,
                    ClothingName = x.ClothingName,
                    Collection = x.Collection,
                    CategoryName = x.Category.CategoryName,
                    GenderName = x.Gender.GenderType,
                    Stock = x.Stock,
                    Color = x.Color,
                    Price = x.Price
                });
        }
    }
}
