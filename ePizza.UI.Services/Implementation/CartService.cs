using ePizza.UI.Services.Constants;
using ePizza.UI.Services.Contracts;
using System.Net.Http.Json;

namespace ePizza.UI.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartService()
        {
                
        }


        public async Task<int> UpdateQuantity(Guid CartId, int itemId, int quantity)
        {

            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            var updateCartItems = new
            {
                CartId = CartId,
                ItemId = itemId,
                Quantity = quantity
            };


            var itemResponse
                = await httpClient.PutAsJsonAsync(
                    "api/Cart/update-item", updateCartItems);


            itemResponse.EnsureSuccessStatusCode();

            return 0;
        }

    }
}
