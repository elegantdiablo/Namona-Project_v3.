using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NamonaProject_v3_.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaProjectTest
{
    internal class DbContextFactory
    {
        public static NamonaDbContext Create()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<NamonaDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new NamonaDbContext(options);

            context.Database.EnsureCreated();

            DbSeeder.Seed(context);

            return context;
        }

        public static NamonaDbContext CreateEmpty()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<NamonaDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new NamonaDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
