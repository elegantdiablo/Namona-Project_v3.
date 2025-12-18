using Microsoft.AspNetCore.Authorization;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
namespace NamonaProject_v3_.Model
{
    public class OrderModel
    {
        public NamonaDbContext _context;
        public OrderModel(NamonaDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "Admin")]
        public IEnumerable<OrderDto> AllOrders()
        {
            return _context.orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Address = x.Address
            });
        }
        //[Authorize(Roles = "Admin")]
        public void DeleteOrder(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.orders.Remove(_context.orders.Where(x => x.OrderId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }
}
