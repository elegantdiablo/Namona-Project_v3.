using NamonaProject_v3_.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NamonaMobileApp.Model
{
    public class CategoryModel
    {
        public ApiSession session { get; set; }
        public CategoryModel(ApiSession _session)
        {

            session = _session;
        }

        public async Task<List<AllCategoryDto>> GetAllCategories()
        {
            var res = await session.Client.GetFromJsonAsync<List<AllCategoryDto>>("api/Category/GetAllCategories");
            return res;
        }

        public async Task AddCategory(AddCategoryDto dto)
        {
            var res = await session.Client.PostAsJsonAsync<AddCategoryDto>("api/Category/AddCategory", dto);
            res.EnsureSuccessStatusCode();
        }

        public async Task EditCategory(EditCategoryDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<EditCategoryDto>("api/Category/EditCategory", dto);
            res.EnsureSuccessStatusCode();
        }
        public async Task DeleteCategory(int id)
        {
            var res = await session.Client.DeleteAsync($"api/Cart/DeleteCategory?id={id}");
            res.EnsureSuccessStatusCode();
        }
    }
}
