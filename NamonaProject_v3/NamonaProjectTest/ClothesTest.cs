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
            Assert.All(result, r => Assert.True(r.ClothingId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrWhiteSpace(r.ClothingName)));
        }
        [Fact]
        public async Task ModifyClothesNotFound()
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
            await Assert.ThrowsAsync<InvalidOperationException>(() => _model.ChangeClothingData(dto));
        }

        [Fact]
        public async Task ModifyClothesOK()
        {
            var cartItem = _context.cart.First();
            var newAmount = cartItem.Amount + 1;

            var dto = new ChangeClothingDataDto
            {
                ClothingId = 1,
                ClothingName = "Test",
                Collection = "asd",
                Category = "Test",
                GenderType = "Male",
                Stock = 10,
                Color = "Blue",
                Price = 10000,
            };

            await _model.ChangeClothingData(dto);

            var updatedItem = _context.clothes
                .First(x => 1 == cartItem.ClothingId);

            Assert.Equal("Test", updatedItem.ClothingName);

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
        public async Task AddNewClothOK()
        {
            var amount = _context.clothes.Count();

            var dto = new AddClothesDto
            {

                ClothingName = "Test",
                Collection = "Winter 2025",
                CategoryName = "Hoodie",
                GenderName = "Male",
                Stock = 10,
                Color = "Blue",
                Price = 10000,

            }
            await _model.AddClothes(dto);

            Assert.Equal(_context.clothes.Count(), amount + 1);
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
            Assert.Contains("Kategória üres", ex.Message);
        }
    }
}