namespace NamonaProject_v3_.DTO
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int ClothingId { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public string Category { get; set; }
        public int GenderId { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int PriceSum { get; set; }
    }
}
