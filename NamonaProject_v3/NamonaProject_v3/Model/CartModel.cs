using Microsoft.EntityFrameworkCore;
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
        
        public IEnumerable<CartItemDto> GetCartContent()
        {
            return _context.cart.Include(x=> x.Clothes) .Select(x => new CartItemDto
            {
                CartId = x.CartId,
                ClothingId = x.ClothingId,
                ClothingName = x.Clothes.ClothingName,
                Collection = x.Clothes.Collection,
                Color = x.Clothes.Color,
                Price = x.Clothes.Price,
                Stock = x.Clothes.Stock,
                Amount = x.Amount,
                GenderId = x.Clothes.GenderId,
            });
        }
        
        public void EditCart(int id, CartItemDto dto)
        {
            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().ClothingId;
            using (var trx = _context.Database.BeginTransaction())
                {
                _context.cart.Where(x => x.ClothingId == id).First().Amount = dto.Amount;               
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
