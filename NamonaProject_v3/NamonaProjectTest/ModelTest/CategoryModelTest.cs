using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using NamonaProject_v3_.DTO;

namespace NamonaProjectTest.ModelTest
{
    public class CategoryModelTest
    {
        private readonly CategoryModel _model;
        private readonly NamonaDbContext _context;

        public CategoryModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new CategoryModel(_context);
        }

        [Fact]
        public void AllCategories_Should_Return_Data()
        {
            // Act
            var result = _model.AllCategories().ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, x =>
            {
                Assert.NotNull(x.CategoryName);
                Assert.True(x.Id > 0);
            });
        }

        [Fact]
        public async Task AddCategory_Should_Add_New_Category()
        {
            // Arrange
            var dto = new AddCategoryDto
            {
                CategoryName = "TestCategory"
            };

            // Act
            await _model.AddCategory(dto);

            // Assert
            var added = _context.categories
                .FirstOrDefault(x => x.CategoryName == "TestCategory");

            Assert.NotNull(added);
        }

        [Fact]
        public async Task AddCategory_Should_Throw_When_Name_Null()
        {
            var dto = new AddCategoryDto
            {
                CategoryName = null
            };

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _model.AddCategory(dto));
        }

        [Fact]
        public async Task EditCategory_Should_Update_Name()
        {
            // Arrange
            var category = _context.categories.First();

            var dto = new EditCategoryDto
            {
                Id = category.CategoryId,
                CategoryName = "UpdatedCategory"
            };

            // Act
            await _model.EditCategory(dto);

            // Assert
            var updated = _context.categories
                .First(x => x.CategoryId == category.CategoryId);

            Assert.Equal("UpdatedCategory", updated.CategoryName);
        }

        [Fact]
        public async Task EditCategory_Should_Throw_When_Name_Null()
        {
            var category = _context.categories.First();

            var dto = new EditCategoryDto
            {
                Id = category.CategoryId,
                CategoryName = null
            };

            await Assert.ThrowsAsync<InvalidDataException>(() =>
                _model.EditCategory(dto));
        }

        [Fact]
        public async Task DeleteCategory_Should_Remove_Category()
        {
            // Arrange
            var category = _context.categories.First();

            // Act
            await _model.DeleteCategory(category.CategoryId);

            // Assert
            Assert.False(_context.categories
                .Any(x => x.CategoryId == category.CategoryId));
        }

        [Fact]
        public async Task DeleteCategory_Should_Throw_When_NotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _model.DeleteCategory(999999));
        }
    }
}