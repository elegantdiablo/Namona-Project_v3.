using NamonaProject_v3_.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace NamonaIntegrationTest.ControllerTest
{
    public class UserControllerIntegrationTest : IClassFixture<MyContextFactory>
    {
        private readonly HttpClient _client;
        public UserControllerIntegrationTest(MyContextFactory factory)
        {

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AdminLogin()
        {
            var dto = new LoginAdminDTO
            {
                UserName = "admin",
                Password = "admin123"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var res = await _client.PostAsync("api/User/admin/login", content);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }

        [Fact]
        public async Task UserLogin()
        {
            var logindto = new LoginDto
            {
                Email = "user@namona.hu",
                Password = "user123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/login", logincontent);
            loginres.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Register_ReturnsOk()
        {
            var dto = new
            {
                Email = "namonaenjoyer@gmail.com",
                UserName = "szeretemanamonat",
                Password = "password123"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/User/register", content);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task Register_ReturnsNotAcceptable()
        {
            var dto = new
            {
                Email = "",
                UserName = "",
                Password = ""
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/User/register", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }
        [Fact]
        public async Task Register_ReturnsConflict()
        {
            var dto = new
            {
                Email = "user@namona.hu",
                UserName = "elegantdiablo",
                Password = "1234567"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/User/register", content);
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
        [Fact]
        public async Task ShowUsers_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };
            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/User/ShowUsers");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task DeleteUser_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/User/DeleteUser?id=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/User/DeleteUser?id=999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsOk()
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
                UserId = 1,
                Password = "newadmin123"
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/User/password", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task UpdatePassword_ReturnsNotAcceptable()
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
                UserId = 1,
                Password = ""
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/User/password", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task PromoteToAdmin_ReturnsOk()
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
                UserId = 2,
                Role = "Admin"
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/User/promote", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task PromoteToAdmin_ReturnsNotAcceptable()
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
                UserId = 2,
                Role = ""
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/User/promote", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);

        }

        [Fact]
        public async Task PromoteToAdmin_ReturnsNotFound()
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
                UserId = 2,
                Role = "Designer"
            };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/User/promote", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task Logout_ReturnsOk()
        {
            var logindto = new LoginDto
            {
                Email = "user@namona.hu",
                Password = "user123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.PostAsync("/api/User/logout", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
