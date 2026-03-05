using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
