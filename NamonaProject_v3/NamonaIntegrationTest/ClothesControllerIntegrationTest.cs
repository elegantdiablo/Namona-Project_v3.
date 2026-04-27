using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NamonaIntegrationTest
{
    public class ClothesControllerIntegrationTest : IClassFixture<MyContextFactory>
    {
        private readonly HttpClient _client;

        public ClothesControllerIntegrationTest(MyContextFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllClothes_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Clothes/GetAllClothes");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddClothes_ReturnsCreated()
        {
            var dto = new
            {
                ClothingName = "Integration Shirt",
                Collection = "2026",
                Size = "M",
                GenderId = 1,
                CategoryId = 1,
                Stock = 10,
                Color = "Black",
                Price = 9990
            };

            var response = await _client.PostAsJsonAsync("/api/Clothes/AddClothes", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task ModifyClothes_ReturnsOk()
        {
            var dto = new
            {
                ClothingId = 1,
                ClothingName = "Updated Shirt",
                Collection = "Updated",
                Size = "L",
                GenderId = 1,
                CategoryId = 1,
                Stock = 20,
                Color = "White",
                Price = 10990
            };

            var response = await _client.PostAsJsonAsync("/api/Clothes/modify", dto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteClothes_ReturnsOk()
        {
            var response = await _client.DeleteAsync("/api/Clothes/remove?id=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task FilterClothes_ReturnsOk()
        {
            var dto = new
            {
                CategoryId = 1,
                GenderId = 1,
                MinPrice = 0,
                MaxPrice = 20000,
                Size = "M"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Clothes/FilterClothes")
            {
                Content = JsonContent.Create(dto)
            };

            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SearchClothes_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Clothes/SearchClothes?text=Namona");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}