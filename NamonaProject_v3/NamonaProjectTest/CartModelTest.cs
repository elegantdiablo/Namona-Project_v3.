using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NamonaProjectTest
{
    public class CartModelTest
    {
        private readonly CartModel _model;
        private readonly NamonaDbContext _context;

        public CartModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new CartModel(_context);
        }

        [Fact]
        public void GetCartData_Valid()
        {
            var result = _model.GetCartData().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.CartId > 0));
            Assert.All(result, r => Assert.True(r.UserId > 0));
        }

        [Fact]
        public void GetCartContent_Valid()
        {
            var result = _model.GetCartContent().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.CartId > 0));
            Assert.All(result, r => Assert.True(r.ClothingId > 0));
            Assert.All(result, r => Assert.False(string.IsNullOrEmpty(r.ClothingName)));
            Assert.All(result, r => Assert.True(r.Price >= 0));
        }

        [Fact]
        public async Task EditCart_ValidAmountChange()
        {
            var cartItem = _context.cart.First();
            var newAmount = cartItem.Amount + 1;

            var dto = new CartItemDto
            {
                ClothingId = cartItem.ClothingId,
                Amount = newAmount
            };

            await _model.EditCart(cartItem.ClothingId, dto);

            var updatedItem = _context.cart
                .First(x => x.ClothingId == cartItem.ClothingId);

            Assert.Equal(newAmount, updatedItem.Amount);
        }

        [Fact]
        public async Task DeleteClothes_RemovesItem()
        {
            var clothes = _context.clothes.First();
            int clothingId = clothes.ClothingId;

            await _model.DeleteClothes(clothingId);

            var deleted = _context.clothes
                .FirstOrDefault(x => x.ClothingId == clothingId);

            Assert.Null(deleted);
        }
    }
}
