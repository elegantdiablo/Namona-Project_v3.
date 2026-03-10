using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.Services
{
    internal class ClothesModel
    {
        public ApiSession session { get; set; }
        public ClothesModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task GetAllClothes()
        {
            var res = await session.Client.GetFromJsonAsync("api/AllClothes");
            return res;
        }
    }
}
