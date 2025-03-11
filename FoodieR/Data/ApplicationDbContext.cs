using FoodieR.Models;
using FoodieR.Models.DbObject;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodieR.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Category> Categories { get; set; }//reprezentarea tabelei din baza de date in codul aplicatiei
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
}
