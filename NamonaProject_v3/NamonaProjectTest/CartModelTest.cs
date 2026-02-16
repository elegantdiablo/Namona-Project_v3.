using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaProjectTest
{
    public class CartModelTest
    {
        private readonly CartModel _model;
        private readonly NamonaDbContext _context;

        public CartModelTest()
        {
            _context = DbContextFactory.Create();
            _model = new CartModel(_context);
        }

        [Fact]
        public void GetCartData_Valid()
        {
            var result = _model.GetCartData().ToList();

            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.True(r.CartId > 0));
            Assert.All(result, r => Assert.True(r.UserId > 0));
        }
    }
}
