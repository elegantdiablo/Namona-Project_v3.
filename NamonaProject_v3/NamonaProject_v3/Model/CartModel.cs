using NamonaProject_v3_.Persistance;

namespace NamonaProject_v3_.Model
{
    public class CartModel
    {
        private readonly NamonaDbContext _context;
        public CartModel(NamonaDbContext context)
        {
            _context = context;
        }
    }

}
