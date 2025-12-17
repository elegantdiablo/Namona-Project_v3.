using System.ComponentModel.DataAnnotations;

namespace NamonaProject_v3_.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Address { get; set; }
    }
}
