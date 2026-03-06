using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.DTO;

namespace NamonaMobileApp.Model
{
    public class OrderModel
    {
        public ApiSession session { get; set; }
        public OrderModel(ApiSession _session)
        {

            session = _session;
        }
        public async Task<List<OrderDto>> GetAllOrder()
        {
            var res = await session.Client.GetFromJsonAsync<List<OrderDto>>("api/Order/AllOrders");
            return res;

        }
        public async Task<List<OrderDto>> GetAllOrdersFromUser(int id)
        {
            var res = await session.Client.GetFromJsonAsync<List<OrderDto>>($"api/Order/AllOrders?id={id}");
            return res;

        }
        public async Task AddOrder(OrderDto dto)
        {
            var res = await session.Client.PostAsJsonAsync<OrderDto>("api/Order/AddOrder", dto);
        }

        public async Task UpdateOrder(ChangeClothingDataDto dto)
        {
            var res = await session.Client.PutAsJsonAsync<ChangeClothingDataDto>("api/Order/UpdateOrder", dto);

        }
        public async Task UpdateOrdeStatus(OrderDto dto)
        {
            var res = await session.Client.PutAsJsonAsync($"api/Order/UpdateOrder", dto);

        }
        public async Task CompleteOrder(OrderDto dto)
        {
            var res = await session.Client.PutAsJsonAsync($"api/Order/UpdateOrder", dto);

        }

        public async Task DeleteOrder(int id)
        {
            var res = await session.Client.DeleteFromJsonAsync($"api/Order/cancel?id={id}", null);
        }
    }
}
