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

        public IEnumerable<OrderDto> PreviousOrders()
        {
            return _context.orders.Select(x => new OrderDto
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                Address = x.Address
            }).ToList();
        }


    }
}
