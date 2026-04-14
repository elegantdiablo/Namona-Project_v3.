using System.ComponentModel.DataAnnotations;
using System;

namespace NamonaAvalonia.DTO
{
    public class AddOrderDto
    {

        public DateTimeOffset OrderDate { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
