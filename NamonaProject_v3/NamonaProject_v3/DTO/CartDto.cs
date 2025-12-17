namespace NamonaProject_v3_.DTO
{
    public class CartDto
    {
        public int UserId { get; set; }

        public List<CartItemDto> Items { get; set; } = new();

        public int TotalAmount { get; set; }

        public int TotalPrice { get; set; }
    }

    public class CartItemDto
    {
        public int CartId { get; set; }

        public int ClothingId { get; set; }

        public int Amount { get; set; }

        public int PriceSum { get; set; }
    }
}
