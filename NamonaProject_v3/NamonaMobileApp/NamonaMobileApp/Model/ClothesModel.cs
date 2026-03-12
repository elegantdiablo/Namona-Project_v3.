using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class ClothesModel
    {
        public ApiSession session { get; set; }
        public ClothesModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<AllClothesDto>> GetAllClothes()
        {
            var res = await session.Client.GetFromJsonAsync<List<AllClothesDto>>("api/Clothes/AllClothes");
            return res;
        
        }
        public async Task AddClothes(AddClothesDto dto)
        {
            var res = await session.Client.PostAsJsonAsync<AddClothesDto>("api/Clothes/add", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task EditClothes(ChangeClothingDataDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<ChangeClothingDataDto>("api/Clothes/modify", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task RemoveClothes(int id)
        {
            var res = await session.Client.DeleteAsync($"api/Clothes/remove?id={id}");
            res.EnsureSuccessStatusCode();
        }
    }
}
