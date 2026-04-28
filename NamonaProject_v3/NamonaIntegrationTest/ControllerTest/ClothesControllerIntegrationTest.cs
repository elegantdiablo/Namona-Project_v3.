using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Microsoft.VisualStudio.CodeCoverage;
using NamonaProject_v3_.DTO;

namespace NamonaProjectTest
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
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingName = "Integration Shirt",
                Collection = "2026",
                Size = "M",
                GenderName = "Male",
                CategoryName = "T-Shirt",
                Stock = 10,
                Color = "Black",
                Price = 9990
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Clothes/AddClothes", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task AddClothes_ReturnsNotAcceptable()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingName = "",
                Collection = "2026",
                Size = "M",
                GenderName = "Male",
                CategoryName = "T-Shirt",
                Stock = 10,
                Color = "Black",
                Price = 9990
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Clothes/AddClothes", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task AddClothes_ReturnsConflict()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingName = "Namona Oversized Hoodie",
                Collection = "2026",
                Size = "M",
                GenderName = "Male",
                CategoryName = "T-Shirt",
                Stock = 10,
                Color = "Black",
                Price = 9990
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Clothes/AddClothes", content);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }


        [Fact]
        public async Task AddClothes_ReturnsNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingName = "Integration Shirt",
                Collection = "2026",
                Size = "M",
                GenderName = "BiSexual",
                CategoryName = "Piercing",
                Stock = 10,
                Color = "Black",
                Price = 9990
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Clothes/AddClothes", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ModifyClothes_ReturnsOk()
        {

            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
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
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Clothes/modify", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModifyClothes_ReturnsNotAcceptable()
        {

            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingId = 1,
                ClothingName = "",
                Collection = "Updated",
                Size = "L",
                GenderId = 1,
                CategoryId = 1,
                Stock = 20,
                Color = "White",
                Price = 10990

            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Clothes/modify", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task ModifyClothes_ReturnsConflict()
        {

            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var dto1 = new
            {
                ClothingName = "Fancy Shirt",
                Collection = "2026",
                Size = "M",
                GenderName = "Male",
                CategoryName = "T-Shirt",
                Stock = 10,
                Color = "Black",
                Price = 9990
            };
            var content = new StringContent(JsonSerializer.Serialize(dto1), Encoding.UTF8, "application/json");
            var createdresponse = await _client.PostAsync("api/Clothes/AddClothes", content);
            createdresponse.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingId = 1,
                ClothingName = "Fancy Shirt",
                Collection = "Updated",
                Size = "L",
                GenderId = 1,
                CategoryId = 1,
                Stock = 20,
                Color = "White",
                Price = 10990

            };
            var modifycontent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Clothes/modify", modifycontent);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }


        [Fact]
        public async Task ModifyClothes_ReturnsNotFound()
        {

            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();
            var dto = new
            {
                ClothingId = 1,
                ClothingName = "Updated Shirt",
                Collection = "Updated",
                Size = "L",
                GenderId = 200,
                CategoryId = 200,
                Stock = 20,
                Color = "White",
                Price = 10990

            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Clothes/modify", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteClothes_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Clothes/remove?id=3");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteClothes_ReturnsNotfound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Clothes/remove?id=0");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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