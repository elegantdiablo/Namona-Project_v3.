using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace NamonaProjectTest.ControllerTest
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
            var dto = new
            {
                UserName = "admin",
                Password = "admin123"
            };

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var res = await _client.PostAsync("api/User/admin/login", content);
            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        }
    }
}
