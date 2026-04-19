using NamonaAvalonia.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
        public async Task LogIn(LoginAdminDTO dto)
        {
            var response = await _client.PostAsJsonAsync("api/user/admin/login", dto);
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<AllClothesDto>> GetAllClothes()
        {
            var res = await _client.GetFromJsonAsync<List<AllClothesDto>>("api/Clothes/GetAllClothes");
            return res;

        }
        public async Task AddClothes(AddClothesDto dto)
        {
            var res = await _client.PostAsJsonAsync<AddClothesDto>("api/Clothes/add", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task EditClothes(ChangeClothingDataDto dto)
        {
            var res = await _client.PutAsJsonAsync<ChangeClothingDataDto>("api/Clothes/modify", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task RemoveClothes(int id)
        {
            var res = await _client.DeleteAsync($"api/Clothes/remove?id={id}");
            res.EnsureSuccessStatusCode();
        }
        
        public async Task PromoteToAdmin(int id)
        {
            var res = await _client.PutAsync($"api/user/{id}/promote", null);
            res.EnsureSuccessStatusCode();
        }
        public async Task<List<CartItemDto>> GetAllCart()
        {
            var res = await _client.GetFromJsonAsync<List<CartItemDto>>("api/Cart/CartContent");
            return res;
        }
        public async Task AddCart(AddToCartDto dto)
        {
            var res = await _client.PostAsJsonAsync<AddToCartDto>("api/Cart/addCart", dto);
            res.EnsureSuccessStatusCode();
        }
        public async Task EditCart(EditCartDto dto)
        {
            var res = await _client.PutAsJsonAsync<EditCartDto>("api/Cart/EditCart", dto);
            res.EnsureSuccessStatusCode();
        }
        public async Task DeleteCart(int id)
        {
            var res = await _client.DeleteAsync($"api/Cart/DeleteCartItem?id={id}");
            res.EnsureSuccessStatusCode();
        }
        public async Task<List<OrderDto>> GetAllOrder()
        {
            var res = await _client.GetFromJsonAsync<List<OrderDto>>("api/Orders/AllOrders");
            return res;

        }
        public async Task<List<OrderDto>> GetAllOrdersFromUser(int id)
        {
            var res = await _client.GetFromJsonAsync<List<OrderDto>>($"api/Orders/AllOrders?id={id}");
            return res;

        }
        public async Task AddOrder(OrderDto dto)
        {
            var res = await _client.PostAsJsonAsync<OrderDto>("api/Orders/AddOrder", dto);
            res.EnsureSuccessStatusCode();
        }

        public async Task UpdateOrder(OrderDto dto)
        {
            var res = await _client.PutAsJsonAsync<OrderDto>("api/Orders/UpdateOrder", dto);
            res.EnsureSuccessStatusCode();

        }
        public async Task UpdateOrdeStatus(OrderDto dto)
        {
            var res = await _client.PutAsJsonAsync($"api/Orders/UpdateOrder", dto);
            res.EnsureSuccessStatusCode();

        }
        
        public async Task CompleteOrder(OrderDto dto)
        {
            var res = await _client.PutAsJsonAsync($"api/Orders/UpdateOrder", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task DeleteOrder(int id)
        {
            var res = await _client.DeleteAsync($"api/Orders/cancel?id={id}");
            res.EnsureSuccessStatusCode();
        }
        public async Task<List<AllCategoryDto>> GetAllCategories()
        {
            var res = await _client.GetFromJsonAsync<List<AllCategoryDto>>("api/Category/GetAllCategories");
            return res;
        }

        public async Task AddCategory(AddCategoryDto dto)
        {
            var res = await _client.PostAsJsonAsync<AddCategoryDto>("api/Category/AddCategory", dto);
            res.EnsureSuccessStatusCode();
        }

        public async Task EditCategory(EditCategoryDto dto)
        {
            var res = await _client.PutAsJsonAsync<EditCategoryDto>("api/Category/EditCategory", dto);
            res.EnsureSuccessStatusCode();
        }
        public async Task DeleteCategory(int id)
        {
            var res = await _client.DeleteAsync($"api/Category/DeleteCategory?id={id}");
            res.EnsureSuccessStatusCode();
        }
        public async Task<List<AllGendersDto>> GetAllGenders()
        {
            var res = await _client.GetFromJsonAsync<List<AllGendersDto>>("api/Gender/AllGenders");
            return res;

        }
        public async Task AddGender(AddGenderDto dto)
        {
            var res = await _client.PostAsJsonAsync<AddGenderDto>("api/Gender/AddGender", dto);
            res.EnsureSuccessStatusCode();
        }

        public async Task EditGender(EditGenderDto dto)
        {
            var res = await _client.PutAsJsonAsync<EditGenderDto>("api/Gender/ModifyGender", dto);
            res.EnsureSuccessStatusCode();

        }

        public async Task DeleteGender(int id)
        {
            var res = await _client.DeleteAsync($"api/Gender/DeleteGender?id={id}");
            res.EnsureSuccessStatusCode();
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var res = await _client.GetFromJsonAsync<List<UserDto>>("api/User/ShowUsers");
            return res;
        }

        public async Task EditUser(UserDto dto)
        {
            var res = await _client.PutAsJsonAsync<UserDto>("api/user/EditUser", dto);
            res.EnsureSuccessStatusCode();
        }

        public async Task DeleteUser(int id)
        {
            var res = await _client.DeleteAsync($"api/user/{id}");
            res.EnsureSuccessStatusCode();
        }
    }
}
