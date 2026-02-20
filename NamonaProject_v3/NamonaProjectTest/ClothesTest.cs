using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace NamonaProjectTest
{
    public class ClothesTest
    {
        private readonly ClothesModel _model;
        private readonly NamonaDbContext _context;

        public ClothesTest()
        {
            _context = DbContextFactory.Create();
            _model = new ClothesModel(_context);
        }
        [Fact]
        public void AllClothes_Valid()
        {
            var result = _model.GetAllClothes().ToList();

            Assert.NotEmpty(result);
            Assert.Contains(result, r => r.ClothingName == "");
            Assert.All(result, r => Assert.True(r.ClothingId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrWhiteSpace(r.ClothingName)));
        }
        [Fact]
        public void AllClothes_NotValid()
        {
            using var empty = DbContextFactory.CreateEmpty();
            var model = new ClothesModel(empty);

            var ex = Assert.Throws<KeyNotFoundException>(() => model.GetAllClothes().ToList());
            Assert.Contains("Nincs ilyen ruha", ex.Message);
        }
        [Fact]
        public async Task ModifyClothes()
        {
            var dto = new ChangeClothingDataDto
            {
                ClothingId = 999,
                ClothingName = "Test",
                Collection = "asd",
                Category = "Test",
                GenderType = "Male",
                Stock = 10,
                Color = "Blue",
                Price = 10000,
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.ChangeClothingData(dto.ClothingId, dto));
        }
        [Theory]
        [InlineData(null)]
        public async Task DeleteClothes_ArgOutRange(int id)
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.DeleteClothes(id));
        }
        [Fact]
        public async Task DeleteClothes_ExistingId()
        {
            var id = _context.clothes
                .Where(r => r.ClothingName == "Hoodie")
                .Select(r => r.ClothingId)
                .First();

            var before = _context.clothes.Count();

            await _model.DeleteClothes(id);

            var after = _context.clothes.Count();
            Assert.Equal(before - 1, after);
            Assert.False(_context.clothes.Any(r => r.ClothingId == id));
        }
        [Theory]
        [InlineData]
        public async Task AddNewCloth_ArgumentNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _model.AddClothes(null!));
        }
        [Fact]
        public async Task DeleteCloth_KeyNotFound()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _model.DeleteClothes(999999));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task AddNewCloth_ArgumentEx(string badName)
        {
            var dto = new AddClothesDto
            {
                CatgeroryId = 1,
                CategoryName = badName,
                GenderId = 1,
                GenderName = badName,
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _model.AddClothes(dto));
            Assert.Contains("CategoryName nem lehet üres", ex.Message);
        }
    }
}