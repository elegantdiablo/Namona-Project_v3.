using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

namespace NamonaProjectTest
{
    public class OrderModelTest
    {
        private readonly OrderModel _model;
        private readonly NamonaDbContext _context;

        public OrderModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new OrderModel(_context);
        }

        [Fact]
        public void AllOrders_Valid()
        {

            var result = _model.AllOrders().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.OrderId >= 0));
            Assert.All(result, r => Assert.False(string.IsNullOrWhiteSpace(r.Address)));
        }



        [Fact]
        public void MyCart_valid()
        {
            int userid = _context.orders.Min(x => x.OrderId);
            var result = _model.MyCart(userid);

            Assert.Equal(result.UserId, userid);
            //Assert.NotEmpty(result.Carts);

        }
    }
}
