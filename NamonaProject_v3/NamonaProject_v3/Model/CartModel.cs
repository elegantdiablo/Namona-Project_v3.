using NamonaProject_v3_.DTO;
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

        public IEnumerable<CartDto> GetCartData()
        {
            return _context.cart.Select(x => new CartDto
            {
                CartId = x.CartId,
                UserId = x.UserId,
            });
        }
        /* 
        public IEnumerable<CartItemDto> GetCartContent()
        {
            return _context.cart.Select(x => new CartItemDto
            {
                CartId = x.CartId,
                ClothingId = x.ClothingId,
                ClothingName = x.clothes.ClothingName,
                Category = x.Category,
                Collection = x.Collection,
                Color = x.Color,
                Price = x.Price,
                Stock = x.Stock,
                GenderId = x.GenderId,
            });
        }
        */
        public void EditCart(int id, ChangeClothingDataDto dto)
        {
            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingName).First().ClothingId;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Where(x => x.ClothingId == id).First().ClothingName = dto.ClothingName;
                _context.clothes.Where(x => x.ClothingId == id).First().Collection = dto.Collection;
                _context.clothes.Where(x => x.ClothingId == id).First().Category = dto.Category;
                _context.clothes.Where(x => x.ClothingId == id).First().Color = dto.Color;
                _context.clothes.Where(x => x.ClothingId == id).First().Price = dto.Price;
                _context.clothes.Where(x => x.ClothingId == id).First().GenderId = _context.genders.Where(x => x.GenderType == dto.GenderType).First().GenderId;
                _context.clothes.Where(x => x.ClothingId == id).First().Stock = dto.Stock;

                _context.SaveChanges();
                trx.Commit();
            }
        }

        public void DeleteClothes(int id)
        {
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                _context.SaveChanges();
                trx.Commit();
            }
        }
    }

}
