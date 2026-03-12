using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class GenderModel
    {
        public ApiSession session { get; set; }
        public GenderModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<AllGendersDto>> GetAllGenders()
        {
            var res = await session.Client.GetFromJsonAsync<List<AllGendersDto>>("api/Gender/AllGenders");
            return res;

        }
        public async Task AddGender(AddGenderDto dto)
        {
            var res = await session.Client.PostAsJsonAsync<AddGenderDto>("api/Gender/AddGender", dto );
            res.EnsureSuccessStatusCode();
        }

        public async Task EditGender(EditGenderDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<EditGenderDto>("api/Gender/ModifyGender", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task DeleteGender(int id)
        {
            var res = await session.Client.DeleteAsync($"api/Gender/DeleteGender?id={id}");
            res.EnsureSuccessStatusCode();
        }
    }
}
