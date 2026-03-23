using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Data.Repositories;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Business.Services;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext (Database)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))
    ));

// Register Repository (DAO Layer)
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();

// Register Service (Business Layer)
builder.Services.AddScoped<IIngredientService, IngredientService>();

// Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();