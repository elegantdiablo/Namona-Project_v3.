using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    internal class ClothesModel
    {
        public ApiSession session { get; set; }
        public ClothesModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<AllClothesDto>> GetAllClothes()
        {
            var res = await session.Client.GetFromJsonAsync<List<AllClothesDto>>("api/AllClothes");
            return res;
        
        }
        public async Task<AddClothesDto> AddClothes(AddClothesDto dto)
        {
            var res = await session.Client.GetFromJsonAsync<AddClothesDto>("api/add");
            return res;
        }

        public async Task<EditCartDto> EditClothes(ChangeClothingDataDto dto)
        {
            var res = await session.Client.GetFromJsonAsync<ChangeClothingDataDto>("api/modify");
            return res;
        }
    }
}
