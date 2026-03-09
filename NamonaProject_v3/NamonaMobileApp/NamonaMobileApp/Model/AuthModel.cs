using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class AuthModel
    {

        public readonly ApiSession _session;

        public AuthModel(ApiSession session)
        {
            _session = session;
        }
    

        public async Task<HttpResponseMessage> Login(string email, string password)
        {
            var res = await _session.Client.PostAsJsonAsync<UserDto>($"api/user/admin/login?username={email}&password={password}", null);

            if (!res.IsSuccessStatusCode)
                return res;
            var user = await res.Content.ReadFromJsonAsync<UserDto>();

            _session.Userid = Convert.ToInt32(user.UserId);
            _session.Username = user.Email;
            _session.Role = user.Role;

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
    }
}
