using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaIntegrationTest.ControllerTest
{
    public class CartControllerIntegrationTest : IClassFixture<MyContextFactory>
    {
        private readonly HttpClient _client;

        public CartControllerIntegrationTest(MyContextFactory factory)
        {

            _client = factory.CreateClient();
        }


        [Fact]
        public async Task GetCartContent_ReturnsOk()
        {
            var logindto = new LoginDto
            {
                Email = "user@namona.hu",
                Password = "user123"
            };

            var logincontent = new StringContent(JsonSerializer.Serialize(logindto), Encoding.UTF8, "application/json");
            var loginres = await _client.PostAsync("api/User/login", logincontent);
            loginres.EnsureSuccessStatusCode();


            var response = await _client.GetAsync("/api/Cart/CartContent?userid=1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
