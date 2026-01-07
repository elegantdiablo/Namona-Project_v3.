using Microsoft.AspNetCore.Authorization;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using Microsoft.EntityFrameworkCore;

namespace NamonaProject_v3_.Model
{
    public class OrderModel
    {
        public NamonaDbContext _context;
        public OrderModel(NamonaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDto> AllOrders()
        {
            return _context.orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Address = x.Address
            });
        }

        public void DeleteOrder(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.Remove(_context.orders.Where(x => x.OrderId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void AddOrder(OrderDto order)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.Add(new Persistance.Orders
                {
                    OrderDate = order.OrderDate,
                    Address = order.Address
                });
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateOrder(int id, OrderDto order)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                var existingOrder = _context.orders.Where(x => x.OrderId == id).First();
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.Address = order.Address;
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void CancelOrder(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                var order = _context.orders.Where(x => x.OrderId == id).First();
                _context.orders.Remove(order);
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void CompleteOrder(int id)
        {
            var order = _context.orders.FirstOrDefault(x => x.OrderId == id);

            if (order == null)
                throw new Exception("Order not found");

            order.Status = "Completed";
            order.CompletedAt = DateTime.UtcNow;

            _context.SaveChanges();
        }


        public void ClearOrders()
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.RemoveRange(_context.orders);
                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void UpdateOrderStatus(int id, string status)
        {
            var order = _context.orders.FirstOrDefault(x => x.OrderId == id);
            if (order == null)
                throw new Exception("Order not found");
            order.Status = status;
            _context.SaveChanges();
        }

    }
}