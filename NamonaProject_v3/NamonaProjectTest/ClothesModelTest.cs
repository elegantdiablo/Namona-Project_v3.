using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using NamonaProject_v3_.DTO;
using Namona_v3.DTO;

namespace NamonaProjectTest
{
    public class ClothesModelTest
    {
        private readonly ClothesModel _model;
        private readonly NamonaDbContext _context;

        public ClothesModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new ClothesModel(_context);
        }

        [Fact]
        public void GetAllClothes_Should_Return_Data()
        {
            var result = _model.GetAllClothes().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, x =>
            {
                Assert.NotNull(x.ClothingName);
                Assert.NotNull(x.CategoryName);
                Assert.NotNull(x.GenderType);
            });
        }

        [Fact]
        public async Task AddClothes_Should_Add_New_Item()
        {
            // Arrange
            var category = _context.categories.First();
            var gender = _context.genders.First();

            var dto = new AddClothesDto
            {
                ClothingName = "TestCloth",
                Collection = "Summer",
                CategoryName = category.CategoryName,
                GenderName = gender.GenderType,
                Size = "M",
                Stock = 5,
                Color = "Black",
                Price = 100
            };

            // Act
            await _model.AddClothes(dto);

            // Assert
            var added = _context.clothes.FirstOrDefault(x => x.ClothingName == "TestCloth");
            Assert.NotNull(added);
            Assert.Equal(100, added.Price);
        }

        [Fact]
        public async Task AddClothes_Should_Throw_When_Category_Invalid()
        {
            var gender = _context.genders.First();

            var dto = new AddClothesDto
            {
                ClothingName = "Test",
                Collection = "Summer",
                CategoryName = "INVALID",
                GenderName = gender.GenderType,
                Size = "M",
                Stock = 5,
                Color = "Black",
                Price = 100
            };

            await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() =>
                _model.AddClothes(dto));
        }

        [Fact]
        public async Task AddClothes_Should_Throw_When_Invalid_Data()
        {
            var category = _context.categories.First();
            var gender = _context.genders.First();

            var dto = new AddClothesDto
            {
                ClothingName = null,
                Collection = null,
                CategoryName = category.CategoryName,
                GenderName = gender.GenderType,
                Size = null,
                Stock = -1,
                Color = null,
                Price = 0
            };

            await Assert.ThrowsAsync<System.IO.InvalidDataException>(() =>
                _model.AddClothes(dto));
        }

        [Fact]
        public async Task ChangeClothingData_Should_Update_Item()
        {
            // Arrange
            var clothing = _context.clothes.First();
            var category = _context.categories.First();
            var gender = _context.genders.First();

            var dto = new ChangeClothingDataDto
            {
                ClothingId = clothing.ClothingId,
                ClothingName = "UpdatedName",
                Collection = "UpdatedCollection",
                CategoryId = category.CategoryId,
                Category = category.CategoryName,
                GenderId = gender.GenderId,
                GenderType = gender.GenderType,
                Size = "L",
                Stock = 10,
                Color = "Blue",
                Price = 200
            };

            // Act
            await _model.ChangeClothingData(dto);

            // Assert
            var updated = _context.clothes.First(x => x.ClothingId == clothing.ClothingId);
            Assert.Equal("UpdatedName", updated.ClothingName);
            Assert.Equal(200, updated.Price);
        }

        [Fact]
        public async Task ChangeClothingData_Should_Throw_When_NotFound()
        {
            var category = _context.categories.First();
            var gender = _context.genders.First();

            var dto = new ChangeClothingDataDto
            {
                ClothingId = 999999,
                ClothingName = "Test",
                Collection = "Test",
                CategoryId = category.CategoryId,
                Category = category.CategoryName,
                GenderId = gender.GenderId,
                GenderType = gender.GenderType,
                Size = "M",
                Stock = 1,
                Color = "Red",
                Price = 50
            };

            await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() =>
                _model.ChangeClothingData(dto));
        }

        [Fact]
        public async Task DeleteClothes_Should_Remove_Item()
        {
            // Arrange
            var clothing = _context.clothes.First();

            // Act
            await _model.DeleteClothes(clothing.ClothingId);

            // Assert
            Assert.False(_context.clothes.Any(x => x.ClothingId == clothing.ClothingId));
        }

        [Fact]
        public async Task DeleteClothes_Should_Throw_When_NotFound()
        {
            await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() =>
                _model.DeleteClothes(999999));
        }

        [Fact]
        public void FilterClothes2_Should_Filter_By_Category()
        {
            var category = _context.categories.First();

            var dto = new FilterClothesDto
            {
                Category = category.CategoryName,
                Minprice = 0,
                Maxprice = 999999
            };

            var result = _model.FilterClothes2(dto).ToList();

            Assert.All(result, x =>
                Assert.Equal(category.CategoryName, x.CategoryName));
        }

        [Fact]
        public void FilterClothes2_Should_Filter_By_Price()
        {
            var min = _context.clothes.Min(x => x.Price);
            var max = _context.clothes.Max(x => x.Price);

            var dto = new FilterClothesDto
            {
                Minprice = min,
                Maxprice = max
            };

            var result = _model.FilterClothes2(dto).ToList();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void SearchBar_Should_Return_Results()
        {
            var category = _context.categories.First();

            var result = _model.SearchBar(category.CategoryName).ToList();

            Assert.NotEmpty(result);
        }

        [Fact]
        public void SearchBar_Should_Return_Empty_When_NoMatch()
        {
            var result = _model.SearchBar("zzzzzz_not_found").ToList();

            Assert.Empty(result);
        }
    }
}