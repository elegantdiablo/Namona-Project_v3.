using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NamonaProject_v3_.Persistance;
using Xunit;

namespace NamonaIntegrationTest
{
    public class CartControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        private readonly DbContext _context;

        public CartControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _scope = factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetService<DbContext>();

            if (_context == null)
            {
                throw new InvalidOperationException(
                    "DbContext service not found in the test server's service provider. " +
                    "Replace the call to resolve DbContext with your concrete DbContext type, for example: " +
                    " _scope.ServiceProvider.GetRequiredService<YourConcreteDbContext>()");
            }
        }

        public void Dispose()
        {
            _scope?.Dispose();
            _client?.Dispose();
        }

        [Fact]
        public async Task GetCartContent_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Cart/CartContent?userid=1");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task DeleteCartItem_ReturnsOk_WhenItemExists()
        {
            var user = new Users
            {
                UserName = "Test2",
                Password = "123",
                Email = "test2@test.com",
                Role = "User"
            };

            _context.Set<Users>().Add(user);
            await _context.SaveChangesAsync();

            var cartItem = new Cart
            {
                UserId = user.UserId,
                ClothingId = 1,
                Amount = 1
            };

            _context.Set<Cart>().Add(cartItem);
            await _context.SaveChangesAsync();
            var response = await _client.DeleteAsync($"/api/Cart/DeleteCartItem?id={cartItem.CartId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}