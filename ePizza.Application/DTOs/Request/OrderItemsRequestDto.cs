namespace ePizza.Application.DTOs.Request;

//getting the order items details against the order ID
public class OrderItemsRequestDto
{

    public int ItemId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
}
