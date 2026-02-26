namespace NamonaProject_v3_.DTO
{
    public class CartItemDto : CartDto
    {
        public int ClothingId { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public int CategoryId { get; set; }
        public int GenderId { get; set; }
        public int Stock { get; set; }
        public int Amount { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int? PriceSum { get; set; }
    }
}
