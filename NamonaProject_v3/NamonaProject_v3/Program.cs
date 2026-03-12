using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

var builder = WebApplication.CreateBuilder(args);

// DATABASE
builder.Services.AddDbContextPool<NamonaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connect"))
);

// SERVICES
builder.Services.AddTransient<CartModel>();
builder.Services.AddTransient<ClothesModel>();
builder.Services.AddTransient<UserModel>();
builder.Services.AddTransient<OrderModel>();

// CONTROLLERS
builder.Services.AddControllers();

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTHENTICATION
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/User/login";
        options.LogoutPath = "/api/User/logout";

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

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// SWAGGER
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// CORS
app.UseCors("FrontendPolicy");

// AUTH
app.UseAuthentication();
app.UseAuthorization();

// CONTROLLERS
app.MapControllers();

app.Run();