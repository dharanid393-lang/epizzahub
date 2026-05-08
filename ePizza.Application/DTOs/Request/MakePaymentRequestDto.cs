
namespace ePizza.Application.DTOs.Request
{
    public class MakePaymentRequestDto
    {
        public string PaymentId { get; set; }  =default!;
        public string TransactionId { get; set; } = default!;
        public decimal Tax { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public decimal Total { get; set; } 
        public string Email { get; set; } = default!;
        public string Status { get; set; } = default!;
        public Guid CartId { get; set; }
        public decimal GrandTotal { get; set; }
        public int UserId { get; set; }

        // for capturing order details
        public OrderRequestDto OrderRequest { get; set; }
    }
}
