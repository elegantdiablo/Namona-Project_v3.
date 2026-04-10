using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.Model;
using NamonaProject_v3_.Persistance;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContextPool<NamonaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connect"))
);


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
        policy.WithOrigins(
                "http://localhost:5500",
                "http://127.0.0.1:5500",
                "https://localhost:5500",
                "https://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();


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