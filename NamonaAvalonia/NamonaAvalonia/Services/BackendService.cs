using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NamonaAvalonia.ViewModels;

namespace NamonaAvalonia.Services
{
    public class BackendService : IBackendService
    {
        public Task AddAsync(TableType table, object dto)
        {
            Console.WriteLine($"ADD → {table}");
            Console.WriteLine(JsonSerializer.Serialize(dto));
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TableType table, object dto)
        {
            Console.WriteLine($"UPDATE → {table}");
            Console.WriteLine(JsonSerializer.Serialize(dto));
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TableType table, string id)
        {
            Console.WriteLine($"DELETE → {table} | ID: {id}");
            return Task.CompletedTask;
        }
    }
}