namespace NamonaProject_v3_.Model
{
    public class CartModel
    {
        public int UserId { get; set; }
        public List<CartItemModel> Items { get; set; } = new();
        public int TotalAmount => Items.Sum(i => i.Amount);
        public int TotalPrice => Items.Sum(i => i.PriceSum);
    }
    public class CartItemModel
    {
        public int CartId { get; set; }
        public int ClothingId { get; set; }
        public int Amount { get; set; }
        public int PriceSum { get; set; }
    }
}
