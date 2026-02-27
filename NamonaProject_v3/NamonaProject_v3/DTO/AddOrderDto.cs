using System.ComponentModel.DataAnnotations;

namespace NamonaProject_v3_.DTO
{
    public class AddOrderDto
    {

        public DateTimeOffset OrderDate { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
