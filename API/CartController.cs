using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Request;
using ePizza.Application.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;
        public CartController(ICartService cartService , ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
           
        }

        [HttpGet("get-item-count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetItemCount([FromQuery] Guid cartId)
        {
            _logger.LogInformation($"started the get Item Count execution method inside the cart controller{cartId.ToString()}");

            if (cartId == Guid.Empty)
                return BadRequest("cartId is required.");

            int itemCount = await _cartService.GetItemCountAsync(cartId);

            if (itemCount < 0)
                return NotFound("Cart not found.");

            return Ok(itemCount);
        }

        // This will add the item into the table and update the quantity if item already exists   
        [HttpPost]
        [Route("add-items")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddItems([FromBody] AddItemsDto addRequest)
        {
            var inserted = await _cartService.AddItemsAsync(addRequest);
            return Ok(inserted);
        }

        [HttpGet("get-cart-detail")]
        [AllowAnonymous]
        public async Task<ActionResult<CartResponseDto>> GetCartDetail([FromQuery] Guid cartId)
        {
            if (cartId == Guid.Empty)
                return BadRequest("cartId is required.");

            CartResponseDto? responseDto
                = await _cartService.GetCartDetailsAsync(cartId);
            return Ok(responseDto);
        }

        //this will update the cart with user id when user logs in

        [HttpPut]
        [Route("update-cart-user")]
        public async Task<IActionResult> UpdateCartUser(UpdateCartUserDto userRequest)
        {
            var cartDetails = await _cartService.UpdateCartUserAsync(userRequest);
            return Ok(cartDetails);
        }

        [HttpPut]
        [Route("delete-item")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteItem(DeleteItemFromCartRequestDto deleteItemFromCart)
        {
            var data = await _cartService.DeleteItemFromCartAsync(deleteItemFromCart.CartId, deleteItemFromCart.ItemId);
            return Ok(data);
        }

        [HttpPut]
        [Route("update-item")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateItem(UpdateCartItemRequestDto updateCartItemRequest)
        {
            var data = await _cartService.UpdateItemInCartAsync(
                     updateCartItemRequest.CartId, updateCartItemRequest.ItemId, updateCartItemRequest.Quantity);

            return Ok(data);
        }
    }
}
