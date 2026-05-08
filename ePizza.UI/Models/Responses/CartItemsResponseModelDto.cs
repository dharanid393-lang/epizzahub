namespace ePizza.UI.Models.Responses;

public class CartItemsResponseModelDto
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal Total { get; set; }

    public decimal Tax { get; set; }

    public decimal GrantTotal { get; set; }

    public List<ItemsResponseModelDto> CartItems { get; set; } = new();
}