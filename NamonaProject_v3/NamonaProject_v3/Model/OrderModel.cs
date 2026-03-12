using Microsoft.AspNetCore.Authorization;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.Json.Serialization.Metadata;

namespace NamonaProject_v3_.Model
{
    public class OrderModel
    {
        public NamonaDbContext _context;
        public OrderModel(NamonaDbContext context)
        {
            _context = context;
        }
        public MyCartDto MyCart(int userid)
        {
            if (!_context.orders.Any(x => x.OrderId == userid))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            var carts = _context.cart
                    .Where(c => c.UserId == userid) // csak aktív kosár
                    .Include(c => c.Clothing).ThenInclude(cl => cl.Category)
                    .Include(c => c.Clothing).ThenInclude(cl => cl.Gender)
                     .Select(c => new CartItemDto
                     {
                         ClothingId = c.ClothingId,
                         ClothingName = c.Clothing.ClothingName,
                         Collection = c.Clothing.Collection,
                         CategoryId = c.Clothing.CategoryId,
                         CategoryName = c.Clothing.Category.CategoryName,
                         Color = c.Clothing.Color,
                         Price = c.Clothing.Price,
                         PriceSum = c.PriceSum,
                         Size = c.Clothing.Size,
                         Stock = c.Clothing.Stock,
                         Amount = c.Amount,
                         GenderId = c.Clothing.GenderId,
                         GenderName = c.Clothing.Gender.GenderType
                     }).ToList();

            return new MyCartDto
            {
                UserId = userid,
                Carts = carts
            };
        }

        public IEnumerable<OrderDto> AllOrders()
        {
            return _context.orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                OrderDate = (DateTimeOffset)x.OrderDate,
                Address = x.Address
            });
        }

        public async Task DeleteOrder(int id)
        {
            if (!_context.orders.Any(x => x.OrderId == id))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.Remove(_context.orders.Where(x => x.OrderId == id).First());
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task AddOrder(AddOrderDto order)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.Add(new Persistance.Orders
                {
                    OrderDate = order.OrderDate,
                    Address = order.Address,
                    CompletedAt = null

                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task UpdateOrder(OrderDto order)
        {
            if (!_context.orders.Any(x => x.OrderId == order.OrderId))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            var existingOrder = _context.orders.Where(x => x.OrderId == order.OrderId).First();
            using (var trx = _context.Database.BeginTransaction())
            {
                existingOrder.Address = order.Address;
                existingOrder.Status = order.Status;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }



        public async Task CompleteOrder(int id)
        {
            var order = _context.orders.FirstOrDefault(x => x.OrderId == id);

            if (order == null)
                throw new KeyNotFoundException("Order not found");
            using (var trx = _context.Database.BeginTransaction())
            {
                order.Status = "Completed";
                order.CompletedAt = DateTime.UtcNow;


                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }


        public async Task ClearOrders()
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.RemoveRange(_context.orders);
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task UpdateOrderStatus(UpdateStatusDto dto)
        {
            var order = _context.orders.FirstOrDefault(x => x.OrderId == dto.OrderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found");
            using (var trx = _context.Database.BeginTransaction())
            {
                order.Status = dto.Status;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();

            }
            await Task.CompletedTask;
        }

        /* A stock adatbol kivonunk annyit ahány darab ruhát megrendelt a felhasználó */
        public async Task ModifyStock(int orderid)
        {
            Orders rendeles = _context.orders.Where(x => x.OrderId == orderid).First();

            using (var trx = _context.Database.BeginTransaction())
            {
                List<Clothes> clothes2 = new();
                foreach (Cart item in rendeles.Carts)
                {

                    _context.clothes.Where(x => item.ClothingId == x.ClothingId).First().Stock -= item.Amount;


                }
            }
        }

    }
}