namespace NamonaProject_v3_.DTO
{
    public class OrderDto 
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
