using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class CartModel
    {
        public ApiSession session { get; set; }
        public CartModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<CartItemDto>> GetAllCart()
        {
            var res = await session.Client.GetFromJsonAsync<List<CartItemDto>>("api/Cart/CartContent");
            return res;
        }
        public async Task AddCart(AddToCartDto dto)
        {
            var res = await session.Client.PostAsJsonAsync<AddToCartDto>("api/Cart/addCart", dto);

        }
        public async Task EditCart(EditCartDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<EditCartDto>("api/Cart/EditCart", dto);

        }
        public async Task DeleteCart(int id)
        {
            var res = await session.Client.GetFromJsonAsync<CartItemDto>("api/Cart/DeleteCartItem");

        }

    }
}
