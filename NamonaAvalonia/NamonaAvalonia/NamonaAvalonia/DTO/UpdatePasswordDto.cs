using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.DTO
{
    public class UpdatePasswordDto
    {
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
