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
        }

        public async Task EditClothes(ChangeClothingDataDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<ChangeClothingDataDto>("api/Gender/ModifyGender", dto);
            
        }

        public async Task RemoveClothes(int id)
        {
            var res = await session.Client.DeleteFromJsonAsync($"api/Gender/DeleteGender?id={id}", null);
        }
    }
}
