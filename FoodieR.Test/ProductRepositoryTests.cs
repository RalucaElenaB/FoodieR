using FoodieR.Data;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodieR.Test;

public class ProductRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> GetDbOptions()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: "TestDatabase")
            //.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nume unic pentru fiecare test
             .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void AddProduct_ShouldAddProduct()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            var repository = new ProductRepository(context);
            var product = new Product { Id = 1, Name = "TestProduct", CategoryId = 1 };

            repository.AddProduct(product);

            Assert.Single(context.Products);
            Assert.Equal("TestProduct", context.Products.First().Name);
        }
    }

    [Fact]
    public void GetProducts_ShouldReturnAllProducts()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
           
            context.Products.Add(new Product { Id = 1, Name = "TestProduct1" });
            context.Products.Add(new Product { Id = 2, Name = "TestProduct2" });
            context.Products.Add(new Product { Id = 3, Name = "TestProduct3" });

            var repository = new ProductRepository(context);
            var products = repository.GetProducts();

            Assert.Equal(3, products.Count());
        }
    }
   
}
