namespace NamonaProject_v3_.DTO
{
    public class FilterClothesDto
    {
        public string? Category { get; set; } = null;
        public string? Collection { get; set; } = null;
        public string? Gender { get; set; } = null;
        public int? Minprice { get; set; } = 0;
        public int? Maxprice { get; set; } = 999999;
    }
}
