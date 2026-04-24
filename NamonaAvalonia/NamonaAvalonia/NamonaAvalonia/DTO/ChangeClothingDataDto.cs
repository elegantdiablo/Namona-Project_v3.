namespace NamonaAvalonia.DTO
{
    public class ChangeClothingDataDto
    {
        public int ClothingId { get; set; }
        public string ClothingName { get; set; }
        public string Collection { get; set; }
        public string Size { get; set; }  
        public int Stock { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public int GenderId { get; set; }   
    }
}
