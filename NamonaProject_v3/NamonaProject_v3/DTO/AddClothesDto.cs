namespace NamonaProject_v3_.DTO
{
    public class AddClothesDto : AllClothesDto
    {
        public int CatgeroryId { get; set; }
        public string CatgeroryName { get; set; }
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}
