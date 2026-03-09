using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class UserModel
    {
        public ApiSession session { get; set; }
        public UserModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<UserDto>> GetUsers()
        {
            var result = await session.Client.GetFromJsonAsync<List<UserDto>>("api/user");
            return result;
        }
        public async Task DeleteUser(int id)
        {
            var res = await session.Client.DeleteAsync($"api/user/{id}");
            res.EnsureSuccessStatusCode();
        }
        public async Task PromoteToAdmin(int id)
        {
            var res = await session.Client.PutAsync($"api/user/{id}/promote", null);
            res.EnsureSuccessStatusCode();
        }
    }
}

