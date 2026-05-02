using System.Security.Cryptography;
using System.Text;
using NamonaProject_v3_.Persistance;

namespace NamonaIntegrationTest
{
    public static class DbSeeder
    {
        public static void Seed(NamonaDbContext _db)
        {
            // Ha már van user, akkor feltételezzük hogy seedelt
            if (_db.users.Any())
                return;

            // ============================
            // 1️⃣ Gender
            // ============================
            var male = new Gender { GenderType = "Male" };
            var female = new Gender { GenderType = "Female" };
            var unisex = new Gender { GenderType = "Unisex" };

            _db.genders.AddRange(male, female, unisex);
            _db.SaveChanges();

            // ============================
            // 2️⃣ Category
            // ============================
            var tshirt = new Category { CategoryName = "T-Shirt" };
            var hoodie = new Category { CategoryName = "Hoodie" };
            var pants = new Category { CategoryName = "Pants" };

            _db.categories.AddRange(tshirt, hoodie, pants);
            _db.SaveChanges();

            // ============================
            // 3️⃣ Clothes
            // ============================
            var tee = new Clothes
            {
                ClothingName = "Namona Classic Tee",
                Collection = "Summer 2025",
                Size = "M",
                GenderId = unisex.GenderId,
                CategoryId = tshirt.CategoryId,
                Stock = 100,
                Color = "Black",
                Price = 8990
            };

            var hoodieItem = new Clothes
            {
                ClothingName = "Namona Oversized Hoodie",
                Collection = "Winter 2025",
                Size = "L",
                GenderId = male.GenderId,
                CategoryId = hoodie.CategoryId,
                Stock = 50,
                Color = "Grey",
                Price = 19990
            };

            var pantsItem = new Clothes
            {
                ClothingName = "Namona Slim Pants",
                Collection = "Autumn 2025",
                Size = "S",
                GenderId = female.GenderId,
                CategoryId = pants.CategoryId,
                Stock = 40,
                Color = "Beige",
                Price = 14990
            };

            _db.clothes.AddRange(tee, hoodieItem, pantsItem);
            _db.SaveChanges();

            // ============================
            // 4️⃣ Users
            // ============================
            var admin = new Users
            {
                UserName = "admin",
                Password = HashPassword("admin123"),
                Email = "admin@namona.hu",
                PhoneNumber = "+36111111111",
                Role = "Admin"
            };

            var user = new Users
            {
                UserName = "testuser",
                Password = HashPassword("user123"),
                Email = "user@namona.hu",
                PhoneNumber = "+36222222222",
                Role = "User"
            };

            _db.users.AddRange(admin, user);
            _db.SaveChanges();

            // ============================
            // 5️⃣ Cart
            // ============================
            var cart1 = new Cart
            {
                ClothingId = tee.ClothingId,
                UserId = user.UserId,
                Amount = 2,
                PriceSum = tee.Price * 2
            };

            var cart2 = new Cart
            {
                ClothingId = hoodieItem.ClothingId,
                UserId = user.UserId,
                Amount = 1,
                PriceSum = hoodieItem.Price
            };
            var cart3 = new Cart
            {
                ClothingId = pantsItem.ClothingId,
                UserId = user.UserId,
                Amount = 1,
                PriceSum = pantsItem.Price
            };

            _db.cart.AddRange(cart1, cart2, cart3);
            _db.SaveChanges();

            // ============================
            // 6️⃣ Orders
            // ============================
            var order1 = new Orders
            {
                OrderDate = DateTimeOffset.Now,
                Address = "Budapest, Fő utca 1.",
                Status = "Done",
                CompletedAt = DateTime.Now,
                Carts = new List<Cart> { cart1, cart2, cart3 }
            };
            var order2 = new Orders
            {
                OrderDate = DateTimeOffset.Now,
                Address = "Budapest, Egressy út 71",
                Status = "InProgress",
                CompletedAt = DateTime.Now,
                Carts = new List<Cart> { cart1, cart2, cart3 }
            };
            var order3 = new Orders
            {
                OrderDate = DateTimeOffset.Now,
                Address = "Budapest, Dorozsmai utca 123",
                Status = "InProgress",
                CompletedAt = DateTime.Now,
                Carts = new List<Cart> { cart1, cart2, cart3 }
            };
            var order4 = new Orders
            {
                OrderDate = DateTimeOffset.Now,
                Address = "Budapest, Dorozsmai utca 123",
                Status = "InProgress",
                CompletedAt = DateTime.Now,
                Carts = new List<Cart> { cart1, cart2, cart3 }
            };
            var order5 = new Orders
            {
                OrderDate = DateTimeOffset.Now,
                Address = "Budapest, Dorozsmai utca 123",
                Status = "InProgress",
                CompletedAt = DateTime.Now,
                Carts = new List<Cart> { cart1, cart2, cart3 }
            };
            _db.orders.Add(order1);
            _db.orders.Add(order2);
            _db.orders.Add(order3);
            _db.orders.Add(order4);
            _db.orders.Add(order5);
            _db.SaveChanges();

            // Cartok hozzárendelése az Orderhöz
            cart1.OrderId = order1.OrderId;
            cart2.OrderId = order2.OrderId;
            cart3.OrderId = order3.OrderId;

            _db.cart.UpdateRange(cart1, cart2, cart3);
            _db.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
