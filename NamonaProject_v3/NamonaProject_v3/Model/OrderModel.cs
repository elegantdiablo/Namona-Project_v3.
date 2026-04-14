using Microsoft.AspNetCore.Authorization;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NamonaProject_v3_.Model
{
    public class OrderModel
    {
        public NamonaDbContext _context;
        public OrderModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderHistoryDto> GetOrdersForUser(int userid)
        {
            var orderItems = _context.cart
                .Where(c => c.UserId == userid && c.OrderId != null)
                .Include(c => c.Order)
                .Include(c => c.Clothing).ThenInclude(cl => cl.Category)
                .Include(c => c.Clothing).ThenInclude(cl => cl.Gender)
                .Select(c => new
                {
                    OrderId = c.OrderId!.Value,
                    OrderDate = c.Order!.OrderDate,
                    Status = c.Order.Status,
                    Item = new CartItemDto
                    {
                        CartId = c.CartId,
                        UserId = c.UserId,
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
                    }
                })
                .ToList();

            return orderItems
                .GroupBy(x => new { x.OrderId, x.OrderDate, x.Status })
                .OrderByDescending(x => x.Key.OrderDate)
                .Select(x => new OrderHistoryDto
                {
                    OrderId = x.Key.OrderId,
                    OrderDate = x.Key.OrderDate,
                    Status = x.Key.Status,
                    Items = x.Select(y => y.Item).ToList()
                })
                .ToList();
        }

        public async Task<int> CheckoutOrder(int userId, CheckoutOrderDto dto)
        {
            var cartItems = await _context.cart
                .Include(x => x.Clothing)
                .Where(x => x.UserId == userId && x.OrderId == null)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                throw new InvalidOperationException("Cart is empty");
            }

            foreach (var item in cartItems)
            {
                if (item.Amount < 1 || item.Amount > item.Clothing.Stock)
                {
                    throw new InvalidDataException();
                }
            }

            using (var trx = _context.Database.BeginTransaction())
            {
                var order = new Persistance.Orders
                {
                    OrderDate = DateTimeOffset.UtcNow,
                    Address = string.IsNullOrWhiteSpace(dto.Address) ? "Not provided" : dto.Address.Trim(),
                    Status = "Processing",
                    CompletedAt = null,
                    Carts = new List<Cart>()
                };

                _context.orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    item.OrderId = order.OrderId;
                    item.PriceSum = item.Clothing.Price * item.Amount;
                    item.Clothing.Stock -= item.Amount;
                }

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
                return order.OrderId;
            }
        }

        public IEnumerable<OrderDto> AllOrders()
        {
            return _context.orders.Include(x => x.Carts).ThenInclude(x => x.User).Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                Status = x.Status,
                Address = x.Address,
                OrderDate = (DateTimeOffset)x.OrderDate,
                CompletedAt = (DateTimeOffset)x.CompletedAt != null ? (DateTimeOffset)x.CompletedAt : null,  
                UserName = x.Carts.Any()
    ? x.Carts.Select(c => c.User.UserName).FirstOrDefault()
    : "N/A"
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