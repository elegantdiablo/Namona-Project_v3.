using NamonaProject_v3_.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NamonaIntegrationTest.ControllerTest
{
    public class OrderControllerIntegrationTest : IClassFixture<MyContextFactory>
    {
        private readonly HttpClient _client;

        public OrderControllerIntegrationTest(MyContextFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Orders_ReturnsOk()
        {
            var logindto = new LoginDto
            {
                Email = "user@namona.hu",
                Password = "user123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/login", logincontent);
            loginres.EnsureSuccessStatusCode();


            var response = await _client.GetAsync("/api/Orders/Orders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/Orders/AllOrders");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOrderById_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/Orders/GetOrderById?id=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetOrderById_ReturnsNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/Orders/GetOrderById?id=9999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        //Add Order???
        [Fact]
        public async Task EditOrder_ReturnsOk()
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
                OrderId = 1,
                UserName = "ICuby_-",
                OrderDate = DateTimeOffset.Now,
                Address = "Tömő utca 5",
                Status = "InProgress",
                CompletedAt = DateTimeOffset.Now.AddDays(1)

            };



            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Orders/UpdateOrder", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task EditOrder_ReturnsNotAccaptable()
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
                OrderId = 1,
                UserName = "ICuby_-",
                OrderDate = DateTimeOffset.Now,
                Address = "",
                Status = "",
                CompletedAt = DateTimeOffset.Now.AddDays(1)

            };



            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Orders/UpdateOrder", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrderAdmin_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Orders/DeleteOrder?id=2");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrderAdmin_ReturnsNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Orders/DeleteOrder?id=999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteOrderUser_ReturnsOk()
        {
            var logindto = new LoginDto
            {
                Email = "user@namona.hu",
                Password = "user123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.DeleteAsync("/api/Orders/cancel?id=3");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CompleteOrder_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };
            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.PutAsync("/api/Orders/CompleteOrder?id=1", null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CompleteOrder_ReturnsNotFound()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };
            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.PutAsync("/api/Orders/CompleteOrder?id=999", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task EditStatus_ReturnsOk()
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
                OrderId = 1,
                Status = "InProgress"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Orders/status", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task EditStatus_ReturnsNotAcceptable()
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
                OrderId = 1,
                Status = ""
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Orders/status", content);
            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
        }

        [Fact]
        public async Task GetRevenue_ReturnsOk()
        {
            var logindto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/admin/login", logincontent);
            loginres.EnsureSuccessStatusCode();

            var response = await _client.GetAsync("/api/Orders/GetRevenue");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        }
    }
}
