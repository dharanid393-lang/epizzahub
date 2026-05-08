using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public TokenController(ITokenGeneratorService tokenGeneratorService)
        {
            _tokenGeneratorService = tokenGeneratorService;
        }

        [HttpGet]
        [Route("getToken/{userName}/{password}")]
        public async Task<IActionResult> GetToken(string userName, string password)
        {
            var token = await _tokenGeneratorService.GenerateTokenAsync(userName, password);

            return Ok(token);
        }

        [HttpPost]
        [Route("token-refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto requestDto)
        {
            var token = await _tokenGeneratorService.GenerateRefreshToken(requestDto);

            return Ok(token);
        }
    }
}
