using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;

namespace NamonaProject_v3_.Model
{
    public class CategoryModel
    {
        private readonly NamonaDbContext _context;

        public CategoryModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AllCategoryDto> AllCategories()
        {
            return _context.categories.Select(x => new AllCategoryDto
            {
                Id = x.CategoryId,
                CategoryName = x.CategoryName,
            });
        }

        public async Task AddCategory(AddCategoryDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                throw new InvalidDataException();
            }
            if(_context.categories.Any(x => x.CategoryName == dto.CategoryName))
            {
                throw new InvalidOperationException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.categories.Add(new Category
                {
                    CategoryName = dto.CategoryName
                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }
       
        public async Task EditCategory(EditCategoryDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.CategoryName))
            {
                throw new InvalidDataException();
            }
            if (_context.categories.Any(x => x.CategoryName == dto.CategoryName))
            {
                throw new InvalidOperationException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.categories.Where(x => x.CategoryId == dto.Id).First().CategoryName = dto.CategoryName;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task DeleteCategory(int id)
        {
            if(!_context.categories.Any(x => x.CategoryId == id))
            {
                throw new KeyNotFoundException();
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                var categid = _context.categories.Where(x => x.CategoryId == id).First();
                _context.categories.Remove(categid);
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }
    }
}
