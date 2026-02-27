using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.DTO;
using NamonaProject_v3_.Persistance;
using System.Drawing;

namespace NamonaProject_v3_.Model
{
    public class CartModel
    {
        private readonly NamonaDbContext _context;
        public CartModel(NamonaDbContext context)
        {
            _context = context;
        }
        /*
        public IEnumerable<CartDto> GetCartData()
        {
            return _context.cart.Select(x => new CartDto
            {
                CartId = x.CartId,
                UserId = x.UserId,
            });
        }
        */
        public IEnumerable<CartItemDto> GetCartContent(int userid)
        {
            return _context.cart.Include(x => x.Clothing).ThenInclude(c => c.Category)
                .Include(x => x.Clothing).ThenInclude(c => c.Gender)
                .Where(x=> x.UserId == userid)
                .Select(x => new CartItemDto
            {
                CartId = x.CartId,
                ClothingId = x.ClothingId,
                ClothingName = x.Clothing.ClothingName,
                Collection = x.Clothing.Collection,
                CategoryId = x.Clothing.CategoryId,
                Color = x.Clothing.Color,
                Price = x.Clothing.Price,
                PriceSum = x.PriceSum,
                Stock = x.Clothing.Stock,
                Amount = x.Amount,
                GenderId = x.Clothing.GenderId,
                CategoryName = x.Clothing.Category.CategoryName,
                GenderName = x.Clothing.Gender.GenderType
            });
        }

        public async Task AddToCart(AddToCartDto dto)
        {
            if (!_context.cart.Any(x => x.ClothingId == dto.ClothingId))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            if (dto.Amount < 0 && dto.Amount > _context.clothes.Where(x => x.ClothingId == dto.ClothingId).Max(x => x.Stock))
            {
                throw new InvalidDataException();
            }
            int price = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Price * dto.Amount;
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.cart.Add(new Cart
                {
                    ClothingId = dto.ClothingId,
                    UserId = dto.UserId,
                    Amount = dto.Amount,
                    PriceSum = price
                });
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
                await Task.CompletedTask;
            }
        }
        
        
        
        public async Task EditCart( EditCartDto dto)
        {
            if (!_context.cart.Any(x => x.ClothingId == dto.ClothingId))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            if (dto.Amount < 0 && dto.Amount > _context.clothes.Where(x=> x.ClothingId == dto.ClothingId).Max(x=> x.Stock))
            {
                throw new InvalidDataException();
            }

            int Id = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().ClothingId;
            int price = _context.clothes.Where(x => x.ClothingId == dto.ClothingId).First().Price * dto.Amount;

            using (var trx = _context.Database.BeginTransaction())
                {
                _context.cart.Where(x => x.ClothingId == Id).First().Amount = dto.Amount; 
                _context.cart.Where(x => x.ClothingId == Id).First().PriceSum = price ; 
                
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            await Task.CompletedTask;
        }

        public async Task DeleteClothesFromCart(int id)
        {
            if (!_context.cart.Any(x => x.ClothingId == id))
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }
            using (var trx = _context.Database.BeginTransaction())
            {
                _context.clothes.Remove(_context.clothes.Where(x => x.ClothingId == id).First());
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }

            await Task.CompletedTask;
        }
    }
}