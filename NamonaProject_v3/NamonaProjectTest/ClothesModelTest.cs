using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;
using Namona_v3.DTO;
using NamonaProject_v3_.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class ClothesModelTests
{
    private NamonaDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<NamonaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        return new NamonaDbContext(options);
    }

    [Fact]
    public void GetAllClothes_ReturnsAllClothesDtos()
    {
        var context = GetDbContext();
        context.clothes.Add(new Clothes { ClothingId = 1, ClothingName = "Shirt", Collection = "Summer", Size = "M", Color = "Blue", Price = 100, Stock = 10, Gender = new Gender { GenderType = "Male" }, Category = new Category { CategoryName = "Top" } });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var result = model.GetAllClothes().ToList();

        Assert.Single(result);
        Assert.Equal("Shirt", result[0].ClothingName);
    }

    [Fact]
    public async Task ChangeClothingData_ThrowsIfCategoryNotFound()
    {
        var context = GetDbContext();
        context.clothes.Add(new Clothes { ClothingId = 1, ClothingName = "Shirt", Category = new Category { CategoryName = "Top" }, Gender = new Gender { GenderType = "Male" } });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var dto = new ChangeClothingDataDto { ClothingId = 1, Category = "NonExistent", GenderType = "Male" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => model.ChangeClothingData(dto));
    }

    [Fact]
    public async Task ChangeClothingData_ThrowsIfGenderNotFound()
    {
        var context = GetDbContext();
        context.categories.Add(new Category { CategoryId = 1, CategoryName = "Top" });
        context.clothes.Add(new Clothes { ClothingId = 1, ClothingName = "Shirt", Category = context.categories.First(), Gender = new Gender { GenderType = "Male" } });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var dto = new ChangeClothingDataDto { ClothingId = 1, Category = "Top", GenderType = "NonExistent" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => model.ChangeClothingData(dto));
    }

    [Fact]
    public async Task AddClothes_ThrowsIfCategoryNotFound()
    {
        var context = GetDbContext();
        var model = new ClothesModel(context);
        var dto = new AddClothesDto { CategoryName = "NonExistent", GenderName = "Male", ClothingName = "Shirt" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => model.AddClothes(dto));
    }

    [Fact]
    public async Task AddClothes_ThrowsIfGenderNotFound()
    {
        var context = GetDbContext();
        context.categories.Add(new Category { CategoryId = 1, CategoryName = "Top" });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var dto = new AddClothesDto { CategoryName = "Top", GenderName = "NonExistent", ClothingName = "Shirt" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() => model.AddClothes(dto));
    }

    [Fact]
    public async Task DeleteClothes_ThrowsIfClothesNotFound()
    {
        var context = GetDbContext();
        var model = new ClothesModel(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => model.DeleteClothes(999));
    }

    [Fact]
    public void GetCategories_ReturnsAllCategoryDtos()
    {
        var context = GetDbContext();
        context.categories.Add(new Category { CategoryId = 1, CategoryName = "Top" });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var result = model.GetCategories().ToList();

        Assert.Single(result);
        Assert.Equal("Top", result[0].CategoryName);
    }

    [Fact]
    public void FilterClothes2_ReturnsFilteredClothes()
    {
        var context = GetDbContext();
        context.clothes.Add(new Clothes { ClothingId = 1, ClothingName = "Shirt", Collection = "Summer", Price = 100, Stock = 10, Category = new Category { CategoryName = "Top" }, Gender = new Gender { GenderType = "Male" }, Color = "Blue", Size = "M" });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var dto = new FilterClothesDto { Category = "Top", Collection = "Summer", Gender = "Male", Minprice = 50, Maxprice = 150 };
        var result = model.FilterClothes2(dto).ToList();

        Assert.Single(result);
        Assert.Equal("Shirt", result[0].ClothingName);
    }

    [Fact]
    public void SearchBar_ReturnsMatchingClothes()
    {
        var context = GetDbContext();
        context.clothes.Add(new Clothes { ClothingId = 1, ClothingName = "Shirt", Collection = "Summer", Price = 100, Stock = 10, Category = new Category { CategoryName = "Top" }, Gender = new Gender { GenderType = "Male" }, Color = "Blue", Size = "M" });
        context.SaveChanges();

        var model = new ClothesModel(context);
        var result = model.SearchBar("top").ToList();

        Assert.Single(result);
        Assert.Equal("Shirt", result[0].ClothingName);
    }
}