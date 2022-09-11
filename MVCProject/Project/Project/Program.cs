using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Identity.Data;
using Project.Data;
using Project.Infrastructure.Checkout;
using Project.Infrastructure.Photos;
using Project.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProjectContextConnection") ?? throw new InvalidOperationException("Connection string 'ProjectContextConnection' not found.");

builder.Services.AddDbContext<ProjectContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ProjectUser, IdentityRole>()
    .AddEntityFrameworkStores<ProjectContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


//Cloudinary Settings
builder.Services.AddScoped<IPhotoAccessor, PhotoAccessor>();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
StripeConfiguration.ApiKey = "sk_test_51Lg8icI9WKqh9wqK039hFHAK5qENh9qQRLPFWGKj7V858o7SssRt3vLzLPcLk0TMaFQkeILGyUvuS1DvoiYeDHu400l97265TS";

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ProjectContext>();
        var userManager = services.GetRequiredService<UserManager<ProjectUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
        Console.WriteLine("Error duke e ruajtur ne databaz");
    }
}


    app.Run();
