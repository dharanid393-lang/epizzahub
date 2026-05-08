namespace ePizza.UI.Models.Requests
{
    public class AddToCartRequestDto
    {
        public int UserId { get; set; }
        public Guid CartId { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
