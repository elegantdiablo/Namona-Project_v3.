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
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.categories.Add(new Category
                {
                    CategoryId = dto.CategoryId,
                    CategoryName = dto.CategoryName
                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }
       
        public async Task EditCategory(EditCategoryDto dto)
        {
            using(var trx = _context.Database.BeginTransaction())
            {
                _context.categories.Where(x => x.CategoryId == dto.Id).First().CategoryName = dto.CategoryName;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task DeleteCategory(int id)
        {
            using(var trx = _context.Database.BeginTransaction())
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
