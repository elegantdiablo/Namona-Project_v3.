using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace NamonaProject_v3_.Persistance
{
    public class NamonaDbContext : DbContext
    {
        public DbSet<Users> users { get; set; }
        public DbSet<Clothes> clothes { get; set; }
        public DbSet<Cart> cart { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Gender> genders { get; set; }
        public DbSet<Category> categories { get; set; }

        public NamonaDbContext(DbContextOptions<NamonaDbContext> options) : base(options) { }
    }

    public class Clothes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClothingId { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public string Size { get; set; }
        public int GenderId { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public Gender? Gender { get; set; }

    }

    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        public int ClothingId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Amount { get; set; }
        public int? PriceSum { get; set; }
        public Clothes Clothing { get; set; }
        public Users? User { get; set; }
        public int? OrderId { get; set; }
        public Orders? Order { get; set; }
    }
    [Index(nameof(Email), IsUnique =true)]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }    
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        public List<Cart>? Carts { get; set; }
    }

    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        [Required]
        public string Address { get; set; }
        
        public string Status { get; set; } = "InProgress"; // InProgress, Delivery, Done
        public DateTime? CompletedAt { get; set; }
        public List<Cart> Carts { get; set; }
    }
    [Index(nameof(CategoryName), IsUnique = true)]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public List<Clothes>? Clothes { get; set; }
    }
    [Index(nameof(GenderType), IsUnique = true)]
    public class Gender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }
        public string GenderType { get; set; }
        public List<Clothes>? Clothes { get; set; }
    }
}
