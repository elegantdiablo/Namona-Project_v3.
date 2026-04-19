namespace NamonaProject_v3_.DTO
{
    public class CartItemDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ClothingId { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }
        public int Amount { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int? PriceSum { get; set; }
    }
}
