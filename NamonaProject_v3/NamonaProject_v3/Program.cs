using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContextPool<NamonaDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Connect"))
    );}



builder.Services.AddTransient<CartModel>();
builder.Services.AddTransient<ClothesModel>();
builder.Services.AddTransient<UserModel>();
builder.Services.AddTransient<OrderModel>();
builder.Services.AddTransient<GenderModel>();
builder.Services.AddTransient<CategoryModel>();



builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/User/login";
        options.LogoutPath = "/api/User/logout";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.HttpOnly = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = 401;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = 403;
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // In development, allow requests from any origin
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            // In production, restrict to specific origins
            policy.WithOrigins(
                    "http://localhost:5500",
                    "http://127.0.0.1:5500",
                    "https://localhost:5500",
                    "https://127.0.0.1:5500"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
    });
});

var app = builder.Build();


// Seed Admin User on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NamonaDbContext>();
    await SeedAdminUser(dbContext);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("FrontendPolicy");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

<<<<<<< HEAD
async Task SeedAdminUser(NamonaDbContext context)
{
    try
    {
        // Update all existing users with NULL or empty roles to "User"
        var usersWithoutRole = context.users.Where(u => string.IsNullOrEmpty(u.Role)).ToList();
        foreach (var user in usersWithoutRole)
        {
            user.Role = "User";
        }
        if (usersWithoutRole.Any())
        {
            await context.SaveChangesAsync();
            Console.WriteLine($"Updated {usersWithoutRole.Count} users with NULL roles to 'User'");
        }

        // Check if admin user already exists
        var adminExists = context.users.Any(u => u.UserName.ToLower() == "admin" && u.Role == "Admin");
        if (adminExists)
            return;

        // Hash the password
        var adminPassword = "admin";
        var hash = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(adminPassword);
        var hashBytes = hash.ComputeHash(bytes);
        var hashedPassword = Convert.ToBase64String(hashBytes);

        // Create admin user
        var adminUser = new Users
        {
            UserName = "admin",
            Email = "admin@namona.com",
            Password = hashedPassword,
            Role = "Admin"
        };

        context.users.Add(adminUser);
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding admin user: {ex.Message}");
    }
=======
public partial class Program
{

>>>>>>> b6a750cbc40f8a9cf4db5d89d18fbb0432ad1bd8
}