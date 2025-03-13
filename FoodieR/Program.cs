using FoodieR.Data;
using FoodieR.Models;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));// Înregistreaz? baza de date pentru a putea fi injectat?.

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddScoped<ShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));// Adaug ShoppingCart ca serviciu în containerul de dependen?e.
builder.Services.AddSession();//Activeaz? suportul pentru sesiuni în aplica?ie: sesiunile sunt utilizate pentru a p?stra date(CartId) între diferite cereri HTTP.
builder.Services.AddHttpContextAccessor();//Acest serviciu permite accesarea HttpContext( a accesa sesiunea-Session) în ShoppingCart.GetCart() )

builder.Services.AddControllersWithViews();

//Spune sistemului Dependency Injection-DI c? OrderRepository, CategoryRepository etc  trebuie creat o singur? dat? per cerere HTTP; dupa terminarea cererii instanta e distrusa.
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<ShoppingCart> (sp => ShoppingCart.GetCart(sp));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();//o metod? folosit? în Middleware-ul unei aplica?ii ASP.NET Core pentru a activa gestionarea sesiunilor

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

