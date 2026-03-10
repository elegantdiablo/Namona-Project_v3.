using System.Security.Cryptography;
using System.Text;
using NamonaProject_v3_.Persistance;

public static class DbSeeder
{


    public static void Seed(NamonaDbContext _db)
    {
        // Ha már van adat, ne seedeljen újra
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
            Gender = unisex,
            Category = tshirt,
            Stock = 100,
            Color = "Black",
            Price = 8990
        };

        var hoodieItem = new Clothes
        {
            ClothingName = "Namona Oversized Hoodie",
            Collection = "Winter 2025",
            Gender = male,
            Category = hoodie,
            Stock = 50,
            Color = "Grey",
            Price = 19990
        };

        var pantsItem = new Clothes
        {
            ClothingName = "Namona Slim Pants",
            Collection = "Autumn 2025",
            Gender = female,
            Category = pants,
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
            Clothing = tee,
            User = user,
            Amount = 2,
            PriceSum = tee.Price * 2
        };

        var cart2 = new Cart
        {
            Clothing = hoodieItem,
            User = user,
            Amount = 1,
            PriceSum = hoodieItem.Price
        };

        _db.cart.AddRange(cart1, cart2);
        _db.SaveChanges();

        // ============================
        // 6️⃣ Orders
        // ============================
        var order = new Orders
        {
            OrderDate = DateTimeOffset.Now,
            Address = "Budapest, Fő utca 1.",
            Status = "Completed",
            CompletedAt = DateTime.Now,
            Carts = new System.Collections.Generic.List<Cart> { cart1, cart2 }
        };

        _db.orders.Add(order);
        _db.SaveChanges();

        // Frissítjük a Cart elemek Order-jét
        cart1.Order = order;
        cart2.Order = order;
        _db.cart.UpdateRange(cart1, cart2);
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