using ePizza.UI.Models.Responses;

namespace ePizza.UI.Models
{
    public class PaymentModel
    {

        public string Name { get; set; } = default!;
        public string RazorPayKey { get; set; } = default!;
        public string OrderId { get; set; } = default!;
        public decimal GrantTotal {  get; set; }    
        public string Currency { get; set; } = default!;
        public string Receipt { get; set; } = default!;
        public string Description { get; set; } = default!;
        public CartItemsResponseModelDto Cart { get; set; }

    }
}
