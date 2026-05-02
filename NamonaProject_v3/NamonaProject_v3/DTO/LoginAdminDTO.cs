using System.Text.Json.Serialization;

namespace NamonaProject_v3_.DTO
{
    public class LoginAdminDTO
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
