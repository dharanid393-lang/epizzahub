using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Common;
using ePizza.Application.DTOs.Response;
using ePizza.Application.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMemoryCache memoryCache;


        public ItemController(IItemService itemService,IMemoryCache memoryCache)
        {
            _itemService = itemService;
            this.memoryCache = memoryCache;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> Get()
        {
            if(!memoryCache.TryGetValue("Items",out IEnumerable<ItemResponseDto> Items))
            {
                var results = await _itemService.GetItemsAsync();

                memoryCache.Set("Items", results);


                return Ok(results);

            }
         
            //var response = new ApiResponseModelDto<IEnumerable<ItemResponseDto>>(true,results,"Data Fetched");--- instead of this line using concept of middleware
            //to use the same response type in the all API

            return Ok(Items);
        }


        [HttpGet("{id:min(1)}")]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetItemById(int id)
        {
            var results = await _itemService.GetItemByIdAsync(id);

            return Ok(results);
        }
    }
}