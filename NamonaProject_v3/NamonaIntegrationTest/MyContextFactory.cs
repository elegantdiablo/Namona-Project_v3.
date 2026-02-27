using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NamonaProject_v3_.Persistance;

namespace NamonaIntegrationTest
{
    internal class MyContextFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<NamonaDbContext>));
                services.RemoveAll(typeof(IDbContextPool<NamonaDbContext>));
                services.RemoveAll(typeof(IScopedDbContextLease<NamonaDbContext>));

                _connection = new SqliteConnection("Data Source=:memory:");
                _connection.Open();

                services.AddDbContextPool<NamonaDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                    options.EnableSensitiveDataLogging();
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetService<NamonaDbContext>();
                db.Database.EnsureCreated();
                if (!db.clothes.Any())
                {
                    DbSeeder.Seed(db);
                }
            });
            }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
