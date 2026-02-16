using System;
using System.Collections.Generic;
using System.Linq;

namespace NamonaProject_v3_.Persistance
{
    public static class DbSeeder
    {
        public static void Seed(NamonaDbContext db)
        {
            if (db.categories.Any()) return;

            var genders = new List<Gender>
            {
                new Gender { GenderId = 1, GenderType = "Férfi" },
                new Gender { GenderId = 2, GenderType = "Női" },
                new Gender { GenderId = 3, GenderType = "Unisex" }
            };

            db.genders.AddRange(genders);
            db.SaveChanges();

            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Póló" },
                new Category { CategoryId = 2, CategoryName = "Pulóver" },
                new Category { CategoryId = 3, CategoryName = "Kabát" },
                new Category { CategoryId = 4, CategoryName = "Nadrág" },
                new Category { CategoryId = 5, CategoryName = "Kiegészítő" }
            };

            db.categories.AddRange(categories);
            db.SaveChanges();

            var users = new List<Users>
            {
                new Users
                {
                    UserName = "admin",
                    Password = "admin123",
                    Email = "admin@namona.hu",
                    PhoneNumber = 123456789,
                    Role = "Admin"
                },
                new Users
                {
                    UserName = "tesztuser",
                    Password = "user123",
                    Email = "user@namona.hu",
                    PhoneNumber = 987654321,
                    Role = "User"
                }
            };

            db.users.AddRange(users);
            db.SaveChanges();

            var clothes = new List<Clothes>
            {
                new Clothes
                {
                    ClothingName = "Basic White T-Shirt",
                    Collection = "Summer 2025",
                    GenderId = 3,
                    Stock = 50,
                    Color = "Fehér",
                    Price = 4990,
                    CategoryId = 1
                },
                new Clothes
                {
                    ClothingName = "Black Hoodie",
                    Collection = "Winter 2025",
                    GenderId = 1,
                    Stock = 30,
                    Color = "Fekete",
                    Price = 12990,
                    CategoryId = 2
                },
                new Clothes
                {
                    ClothingName = "Blue Jeans",
                    Collection = "Classic",
                    GenderId = 2,
                    Stock = 40,
                    Color = "Kék",
                    Price = 15990,
                    CategoryId = 4
                },
                new Clothes
                {
                    ClothingName = "Leather Jacket",
                    Collection = "Premium",
                    GenderId = 1,
                    Stock = 15,
                    Color = "Barna",
                    Price = 39990,
                    CategoryId = 3
                }
            };

            db.clothes.AddRange(clothes);
            db.SaveChanges();
        }
    }
}
