using ePizza.UI.Models;
using ePizza.UI.Models.Responses;
using ePizza.UI.Models.ViewModels;
using ePizza.UI.TokenHelpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ePizza.UI.Controllers
{
    public class LoginController : Controller
   {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenService _tokenService;

        public LoginController(IHttpClientFactory httpClientFactory ,ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("ePizzaApiClient");

            //instead of this using token handler to use the token in each api call
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenService.GetToken()}");

            //var tokenResponse = await client.GetAsync($"api/Token/getToken/{model.UserName}/{model.Password}");--used without Dto

            var tokenResponse = await client.GetFromJsonAsync<ApiResponseModelDto<TokenResponseModelDto>>($"api/Token/getToken/{model.UserName}/{model.Password}");

            if (tokenResponse is not null && tokenResponse.IsSuccess)
            {
                //// Store tokens in session instead of this created a tokenservice
                //HttpContext.Session.SetString("AccessToken", tokenResponse.Data.AccessToken);
                //HttpContext.Session.SetString("RefreshToken", tokenResponse.Data.RefreshToken);
                //return RedirectToAction("Index", "Home");

                _tokenService.SetToken(tokenResponse.Data.AccessToken);

                var claims = await ProcessToken(tokenResponse.Data.AccessToken);

                bool isAdmin = claims != null && Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "IsAdmin")?.Value);

                if (isAdmin)
                {
                    // 
                }
                else
                {
                    //
                }

                return RedirectToAction("Index", "Home");  //  Navigate to Home Controller
            }


            return View();
        }

        public IActionResult LogOut()
        {             
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        private async Task<List<Claim>> ProcessToken(string accessToken)
        { 
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var claims = new List<Claim>();  
            foreach (var claim in jwtToken.Claims)
            {
                claims.Add(claim);  
            }
            await GenerateTicket(claims);

            return claims;

        }

        //
        private async Task GenerateTicket(List<Claim> claims) { 
        
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // this code will sign the application
            // signINAsync use case-Creates a login cookie that authenticates the user for future requests.
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties()
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }
    }
}
