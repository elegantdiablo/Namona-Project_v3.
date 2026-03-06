using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NamonaAvalonia.Dto;

namespace NamonaAvalonia.Services
{
    public class AuthModel
    {
        public readonly ApiSession _session;

        public AuthModel(ApiSession session)
        {
            _session = session;
        }
        private async Task<MyDataDto> ViewProfile()
        {
            var result = await _session.Client.GetFromJsonAsync<MyDataDto>("api/user/getmydata");
            if (result == null)
                throw new InvalidOperationException("Failed to retrieve user profile data.");
            return result;
        }

        public async Task<HttpResponseMessage> Login(string userame, string password)
        {
            var res = await _session.Client.PostAsync($"api/user/admin/login?username={userame}&password={password}", null);

            if (!res.IsSuccessStatusCode)
                return res;

            var user = await ViewProfile();

            _session.Userid = Convert.ToInt32(user.UserId);
            _session.Username = user.Email;
            _session.Role = user.Role;
            _session.IsAdmin = _session.Role == "Admin";

            return res;
        }

         public async Task Logout()
         {
             var res = await _session.Client.PostAsync("api/user/logout", null);

             res.EnsureSuccessStatusCode();
             _session.Userid = 0;
             _session.Username = "";
             _session.Role = "";

         }

        public async Task<List<UserDto>> GetUsers()
        {
            var res = await _session.Client.GetFromJsonAsync<List<UserDto>>("api/admin");
            return res.ToList();
        }
    }
}
