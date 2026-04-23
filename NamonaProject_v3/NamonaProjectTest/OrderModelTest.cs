using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using NamonaProject_v3_.Persistance;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;

namespace NamonaProjectTest
{
    public class OrderModelTest
    {
        private readonly OrderModel _model;
        private readonly NamonaDbContext _context;

        public OrderModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new OrderModel(_context);
        }

        [Fact]
        public async Task CheckoutOrder_Should_Create_Order_And_Update_Stock()
        {
            // Arrange
            var user = _context.users.First();
            var clothing = _context.clothes.First();

            var cart = new Cart
            {
                UserId = user.UserId,
                ClothingId = clothing.ClothingId,
                Amount = 1
            };

            _context.cart.Add(cart);
            await _context.SaveChangesAsync();

            var dto = new CheckoutOrderDto
            {
                Address = "Test address"
            };

            var initialStock = clothing.Stock;

            // Act
            var orderId = await _model.CheckoutOrder(user.UserId, dto);

            // Assert
            var order = _context.orders.FirstOrDefault(o => o.OrderId == orderId);
            Assert.NotNull(order);
            Assert.Equal("Processing", order.Status);

            var updatedCart = _context.cart.First(c => c.CartId == cart.CartId);
            Assert.Equal(orderId, updatedCart.OrderId);
            Assert.True(updatedCart.PriceSum > 0);

            var updatedClothing = _context.clothes.First(c => c.ClothingId == clothing.ClothingId);
            Assert.Equal(initialStock - 1, updatedClothing.Stock);
        }

        [Fact]
        public async Task CheckoutOrder_Should_Throw_When_Cart_Is_Empty()
        {
            var context = DbContextFactory.CreateEmpty();
            var model = new OrderModel(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                model.CheckoutOrder(1, new CheckoutOrderDto()));
        }

        [Fact]
        public async Task CheckoutOrder_Should_Throw_When_Invalid_Amount()
        {
            // Arrange
            var user = _context.users.First();
            var clothing = _context.clothes.First();

            var cart = new Cart
            {
                UserId = user.UserId,
                ClothingId = clothing.ClothingId,
                Amount = clothing.Stock + 5 // invalid
            };

            _context.cart.Add(cart);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<System.IO.InvalidDataException>(() =>
                _model.CheckoutOrder(user.UserId, new CheckoutOrderDto()));
        }

        [Fact]
        public async Task DeleteOrder_Should_Remove_Order()
        {
            // Arrange
            var order = new Orders
            {
                Address = "Test",
                Status = "Processing",
                OrderDate = DateTimeOffset.UtcNow
            };

            _context.orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _model.DeleteOrder(order.OrderId);

            // Assert
            Assert.False(_context.orders.Any(o => o.OrderId == order.OrderId));
        }

        [Fact]
        public async Task DeleteOrder_Should_Throw_When_Not_Found()
        {
            await Assert.ThrowsAsync<System.Collections.Generic.KeyNotFoundException>(() =>
                _model.DeleteOrder(999999));
        }

        [Fact]
        public async Task CompleteOrder_Should_Update_Status_And_Date()
        {
            // Arrange
            var order = new Orders
            {
                Address = "Test",
                Status = "Processing",
                OrderDate = DateTimeOffset.UtcNow
            };

            _context.orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            await _model.CompleteOrder(order.OrderId);

            // Assert
            var updated = _context.orders.First(o => o.OrderId == order.OrderId);
            Assert.Equal("Completed", updated.Status);
            Assert.NotNull(updated.CompletedAt);
        }

        [Fact]
        public void GetOrdersForUser_Should_Return_Grouped_Orders()
        {
            // Arrange
            var user = _context.users.First();
            var clothing = _context.clothes.First();

            var order = new Orders
            {
                OrderDate = DateTimeOffset.UtcNow,
                Status = "Processing",
                Address = "Test"
            };

            _context.orders.Add(order);
            _context.SaveChanges();

            var cart1 = new Cart
            {
                UserId = user.UserId,
                ClothingId = clothing.ClothingId,
                OrderId = order.OrderId,
                Amount = 1
            };

            var cart2 = new Cart
            {
                UserId = user.UserId,
                ClothingId = clothing.ClothingId,
                OrderId = order.OrderId,
                Amount = 2
            };

            _context.cart.AddRange(cart1, cart2);
            _context.SaveChanges();

            // Act
            var result = _model.GetOrdersForUser(user.UserId).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.First().Items.Count);
        }

        [Fact]
        public async Task UpdateOrderStatus_Should_Update_Status()
        {
            // Arrange
            var order = new Orders
            {
                Address = "Test",
                Status = "Processing",
                OrderDate = DateTimeOffset.UtcNow
            };

            _context.orders.Add(order);
            await _context.SaveChangesAsync();

            var dto = new UpdateStatusDto
            {
                OrderId = order.OrderId,
                Status = "Shipped"
            };

            // Act
            await _model.UpdateOrderStatus(dto);

            // Assert
            Assert.Equal("Shipped", _context.orders.First(o => o.OrderId == order.OrderId).Status);
        }
    }
}