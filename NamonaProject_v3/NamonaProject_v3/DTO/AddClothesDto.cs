namespace NamonaProject_v3_.DTO
{
    public class AddClothesDto 
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public string Size { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
    }
}
