using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NamonaProject_v3_.DTO;

namespace NamonaIntegrationTest.ControllerTest
{
    public class CategoryControllerIntegrationTest : IClassFixture<MyContextFactory>
    {
        private readonly HttpClient _client;

        public CategoryControllerIntegrationTest(MyContextFactory factory)
        {

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/Category/GetAllCategories");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddCategory_ReturnCreated()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var dto = new AddCategoryDto
            {
                CategoryName = "Asdasd"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Category/AddCategory", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task AddCategory_ReturnNotAcceptable()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var dto = new AddCategoryDto
            {
                CategoryName = ""
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Category/AddCategory", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task AddCategory_ReturnConflict()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var dto = new AddCategoryDto
            {
                CategoryName = "Hoodie"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/Category/AddCategory", content);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task ModifyCategory_ReturnsOk()
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
                Id = 1,
                CategoryName = "Skirts"

            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Category/EditCategory", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModifyCategory_ReturnsNotAccaptable()
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
                Id = 1,
                CategoryName = ""

            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Category/EditCategory", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task ModifyCategory_ReturnsConflict()
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
                Id = 1,
                CategoryName = "Hoodie"

            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Category/EditCategory", content);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ReturnOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Category/DeleteCategory?id=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ReturnNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Category/DeleteCategory?id=10");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
