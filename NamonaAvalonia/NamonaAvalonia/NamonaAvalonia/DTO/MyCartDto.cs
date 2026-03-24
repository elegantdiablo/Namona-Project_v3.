using System.Collections;
using System.Collections.Generic;
namespace NamonaAvalonia.DTO
{
    public class MyCartDto
    {
        public int UserId { get; set; }
        public List<CartItemDto> Carts { get; set; }
    }
}
