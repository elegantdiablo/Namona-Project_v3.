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
        public MyCartDto GetCartContent(int userid)
        {
            
            var carts = _context.cart.Include(x => x.Clothing).ThenInclude(c => c.Category)
                .Include(x => x.Clothing).ThenInclude(c => c.Gender)
                .Where(x => x.UserId == userid && x.OrderId == null)
                .Select(x => new CartItemDto
                {
                    CartId = x.CartId,
                    UserId = x.UserId,
                    ClothingId = x.ClothingId,
                    ClothingName = x.Clothing.ClothingName,
                    Collection = x.Clothing.Collection,
                    CategoryId = x.Clothing.CategoryId,
                    Color = x.Clothing.Color,
                    Price = x.Clothing.Price,
                    PriceSum = x.PriceSum,
                    Stock = x.Clothing.Stock,
                    Amount = x.Amount,
                    Size = x.Clothing.Size,
                    GenderId = x.Clothing.GenderId,
                    CategoryName = x.Clothing.Category.CategoryName,
                    GenderName = x.Clothing.Gender.GenderType
                })
                .ToList();

            return new MyCartDto
            {
                UserId = userid,
                Carts = carts
            };
        }

        public IEnumerable<CartItemDto> GetAllCarts()
        {
            return _context.cart.Include(x => x.User).Include(x => x.Clothing).ThenInclude(x => x.Category).Include(x => x.Clothing).ThenInclude(x => x.Gender).Select(x => new CartItemDto
            {
                CartId = x.CartId,
                UserId = x.UserId,
                ClothingId = x.ClothingId,
                ClothingName = x.Clothing.ClothingName,
                Collection = x.Clothing.Collection,
                CategoryId = x.Clothing.CategoryId,
                Color = x.Clothing.Color,
                Price = x.Clothing.Price,
                PriceSum = x.PriceSum,
                Stock = x.Clothing.Stock,
                Amount = x.Amount,
                Size = x.Clothing.Size,
                GenderId = x.Clothing.GenderId,
                CategoryName = x.Clothing.Category.CategoryName,
                GenderName = x.Clothing.Gender.GenderType
            });
        }

        public async Task AddToCart(AddToCartDto dto)
        {
            var clothing = await _context.clothes.FirstOrDefaultAsync(x => x.ClothingId == dto.ClothingId);
            if (clothing == null)
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }

            if (dto.Amount < 1 || dto.Amount > clothing.Stock)
            {
                throw new InvalidDataException();
            }

            var existingCartItem = await _context.cart.FirstOrDefaultAsync(x =>
                x.ClothingId == dto.ClothingId &&
                x.UserId == dto.UserId &&
                x.OrderId == null);

            using (var trx = await _context.Database.BeginTransactionAsync())
            {
                if (existingCartItem != null)
                {
                    var updatedAmount = existingCartItem.Amount + dto.Amount;
                    if (updatedAmount > clothing.Stock)
                    {
                        throw new InvalidDataException();
                    }

                    existingCartItem.Amount = updatedAmount;
                    existingCartItem.PriceSum = clothing.Price * updatedAmount;
                }
                else
                {
                    _context.cart.Add(new Cart
                    {
                        ClothingId = dto.ClothingId,
                        UserId = dto.UserId,
                        Amount = dto.Amount,
                        PriceSum = clothing.Price * dto.Amount
                    });
                }

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
        }
        
        
        
        public async Task EditCart(int userId, EditCartDto dto)
        {
            var cartItem = await _context.cart
                .Include(x => x.Clothing)
                .FirstOrDefaultAsync(x =>
                    x.ClothingId == dto.ClothingId &&
                    x.UserId == userId &&
                    x.OrderId == null);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }

            if (dto.Amount < 1 || dto.Amount > cartItem.Clothing.Stock)
            {
                throw new InvalidDataException();
            }

            using (var trx = await _context.Database.BeginTransactionAsync())
            {
                cartItem.Amount = dto.Amount;
                cartItem.PriceSum = cartItem.Clothing.Price * dto.Amount;

                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
        }

        public async Task DeleteClothesFromCart(int userId, int clothingId)
        {
            var cartItem = await _context.cart.FirstOrDefaultAsync(x =>
                x.ClothingId == clothingId &&
                x.UserId == userId &&
                x.OrderId == null);

            if (cartItem == null)
            {
                throw new KeyNotFoundException("nincs ilyen ruha");
            }

            using (var trx = await _context.Database.BeginTransactionAsync())
            {
                _context.cart.Remove(cartItem);
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
        }
    }
}