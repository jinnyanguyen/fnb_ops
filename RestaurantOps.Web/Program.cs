using Microsoft.EntityFrameworkCore;
using RestaurantOps.Data;
using RestaurantOps.Data.Interfaces;
using RestaurantOps.Data.Repositories;
using RestaurantOps.Business.Interfaces;
using RestaurantOps.Business.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Register DbContext (Database)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0))
    ));

// Register Repository (DAO Layer)
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ISOPRepository, SOPRepository>();
builder.Services.AddScoped<IInventoryTransactionRepository,InventoryTransactionRepository>();

// Register Service (Business Layer)
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISOPService, SOPService>();
builder.Services.AddScoped<ISOPExecutionService,SOPExecutionService>();
builder.Services.AddScoped<IRecipeExecutionService,RecipeExecutionService>();
builder.Services.AddScoped<IInventoryTransactionService,InventoryTransactionService>();

Console.WriteLine("IAuthService Registered Successfully");

// Add MVC
builder.Services.AddControllersWithViews();

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();