using ePizza.UI.Filters;
using ePizza.UI.Models;
using ePizza.UI.Models.Requests;
using ePizza.UI.Models.Responses;
using ePizza.UI.Models.ViewModels;
using ePizza.UI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CartController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        Guid CartId
        {
            get
            {
                Guid id;
                string? cartId = Request.Cookies["CartId"];
                if (cartId == null)
                {
                    id = Guid.NewGuid();
                    Response.Cookies.Append("CartId", id.ToString(), new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(1),
                        Secure = true
                    });
                }
                else
                {
                    id = Guid.Parse(cartId);
                }

                return id;
            }
        }

        public async Task<IActionResult> Index()
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            var cartItems =
                await httpClient.GetFromJsonAsync<ApiResponseModelDto<CartItemsResponseModelDto>>(
                $"api/Cart/get-cart-detail?cartId={CartId}");

            return View(cartItems?.Data);
        }

        [HttpGet("AddToCart/{itemId:int}/{unitPrice:decimal}/{quantity:int}")]
        [LoggingFilter]
        public async Task<JsonResult> AddItemToCart(int itemId, decimal unitPrice, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            var itemResponse
                = await httpClient.PostAsJsonAsync(
                    "api/Cart/add-items", GetCartRequest(itemId, unitPrice, quantity));
            itemResponse.EnsureSuccessStatusCode();

            var itemCount = await GetItemCount(CartId);

            return Json(new { Count = itemCount });
        }

        

        [HttpGet("GetCartCount")]
        public async Task<JsonResult> GetCartCount()
        {
            var count = await GetItemCount(CartId);

            return Json(new { Count = count });
        }

        [HttpPut("UpdateQuantity/{itemId:int}/{quantity:int}")]
        public async Task<JsonResult> UpdateQuantity(int itemId, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            // written these lines instead of creating a response dto for single use             
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

            var itemCount = await GetItemCount(CartId);

            return Json(new { Count = itemCount });
        }


        [HttpDelete("DeleteItem/{itemId:int}")]
        public async Task<JsonResult> DeleteItem(int itemId)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            var deleteCartItem = new
            {
                CartId = CartId,
                ItemId = itemId
            };


            var itemResponse
                = await httpClient.PutAsJsonAsync(
                    "api/Cart/delete-item", deleteCartItem);


            itemResponse.EnsureSuccessStatusCode();

            var itemCount = await GetItemCount(CartId);

            return Json(new { Count = itemCount });
        }

        [HttpGet("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(AddressViewModel request)
        {
            //current user conditon used to check if user is logged in or not
            if (ModelState.IsValid && CurrentUser != null)
            {
                // peform operations

                var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

                // check if the user has items in the cart
                var cartItems = await httpClient.GetFromJsonAsync<ApiResponseModelDto<CartItemsResponseModelDto>>(
                                                         $"api/Cart/get-cart-detail?cartId={CartId}");

                if (cartItems is not null && cartItems.IsSuccess)
                {

                    var cartUserObject =
                        new
                        {
                            CartId,
                            UserId = CurrentUser.UserId
                        };

                    var updateCartUserRequest = await httpClient.PutAsJsonAsync($"api/Cart/update-cart-user", cartUserObject);

                    updateCartUserRequest.EnsureSuccessStatusCode();


                    // preserve address and cart information for payment page
                    // created utility method for this to serialize complex objects for the tempdata

                    TempData.Set("Address", request);
                    TempData.Set("CartDetails", cartItems.Data);

  
                    return RedirectToAction("Index", "Payment");

                }


            }

            return View();

        }






        [NonAction]
        private async Task<int> GetItemCount(Guid cartId)
        {
            using var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

            var itemCount =
                await httpClient.GetFromJsonAsync<ApiResponseModelDto<int>>(
                    $"api/Cart/get-item-count?cartId={CartId}");

            if (itemCount != null) return itemCount.Data;

            return await Task.FromResult(0);
        }

        [NonAction]
        private AddToCartRequestDto GetCartRequest(
          int itemId, decimal unitPrice, int quantity)
        {
            return new AddToCartRequestDto
            {
                CartId = CartId,
                Quantity = quantity,
                ItemId = itemId,
                UnitPrice = unitPrice,
            };
        }
    }
}
