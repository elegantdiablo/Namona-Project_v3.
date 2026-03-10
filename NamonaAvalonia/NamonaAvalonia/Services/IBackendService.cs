using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamonaAvalonia.ViewModels;

namespace NamonaAvalonia.Services
{
    public interface IBackendService
    {
        Task AddAsync(TableType table, object dto);
        Task UpdateAsync(TableType table, object dto);
        Task DeleteAsync(TableType table, string id);
    }
}
