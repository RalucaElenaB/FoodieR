using FoodieR.Data;
using FoodieR.Models;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//inceput builder
var builder = WebApplication.CreateBuilder(args);//prima linie ce se executa cand dai run



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");//Configuration= appsettings.json(o valoare e citita de aici)

//builder.Services= servicii ce vor fi folosite in Controllere prin DI=injectarea dependintelor
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));// Înregistreaza baza de date pentru a putea fi injectata.

builder.Services.AddDatabaseDeveloperPageExceptionFilter();//afiseaza developereului o exceptie cand apare o problema 

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()//adaugare roluri: admin, user etc
    .AddEntityFrameworkStores<ApplicationDbContext>();//IdentityServer(tehnologia)- IdentityUser(clasa implicita pt a crea un cont de user)


builder.Services.AddScoped<ShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));// Adaug ShoppingCart ca serviciu în containerul de dependen?e.
builder.Services.AddSession();//Activeaz? suportul pentru sesiuni în aplica?ie: sesiunile sunt utilizate pentru a p?stra date(CartId) între diferite cereri HTTP.
builder.Services.AddHttpContextAccessor();//Acest serviciu permite accesarea HttpContext( a accesa sesiunea-Session) în ShoppingCart.GetCart() )

builder.Services.AddControllersWithViews();//controllere cu Views=aplicatia are pagini nu returneaza doar date(sau poate avea si si Json=date)

//Spune sistemului Dependency Injection-DI c? OrderRepository, CategoryRepository etc  trebuie creat o singur? dat? per cerere HTTP; dupa terminarea cererii instanta e distrusa.
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ReviewRepository>();
builder.Services.AddScoped<ShoppingCart> (sp => ShoppingCart.GetCart(sp));


//sfarsit builder
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

app.UseHttpsRedirection();//fortam catre https daca url-ul e http
app.UseStaticFiles();//wwwroot = fisiere statice

app.UseSession();//o metod? folosit? în Middleware-ul unei aplica?ii ASP.NET Core pentru a activa gestionarea sesiunilor

app.UseRouting();//rutare

app.UseAuthorization();//roluri


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//cum se face rutarea; {id?} = extra date= query parametri

app.MapRazorPages();//Razor=cod cs+html

//adaugare de roluri
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Manager", "Member" };
    foreach (var role in roles)
    {
        if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
        {
            roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        }
    }
}

app.Run();//aplicatia e pornita

