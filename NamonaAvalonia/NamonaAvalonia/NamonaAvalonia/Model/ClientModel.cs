using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaAvalonia.DTO;

namespace NamonaAvalonia.Model
{
    public class ClientModel
    {
        private HttpClient _client;

        public ClientModel(string port) 
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(port)
            };
        }
        public async Task<UserDto> LogIn(LoginDto dto)
        {
            var response = await _client.PostAsJsonAsync("api/user/admin/login", dto);
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
        public async Task<List<AllClothesDto>> GetAllClothes()
        {
            return await _client.GetFromJsonAsync<List<AllClothesDto>>("api/Clothes/AllClothes");           
        }
    }
}
