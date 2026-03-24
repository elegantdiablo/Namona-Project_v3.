namespace NamonaProject_v3_.DTO
{
    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
    }
}