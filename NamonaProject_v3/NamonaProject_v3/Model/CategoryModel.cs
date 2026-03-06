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




    }
}
