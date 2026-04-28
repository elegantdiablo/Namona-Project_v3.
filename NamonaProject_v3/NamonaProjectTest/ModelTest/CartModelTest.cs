using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NamonaProjectTest.ModelTest
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



    }
}
