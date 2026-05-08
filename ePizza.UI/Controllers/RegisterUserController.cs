using ePizza.UI.Models.ViewModels;
using ePizza.UI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.UI.Controllers
{

    public class RegisterUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegisterUserController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel register)
        {

            if (ModelState.IsValid)
            {
                // create  a http client object

                var client = _httpClientFactory.CreateClient(ApplicationConstants.EPizzaClientName);

                var registerUserRequest
                     = await client.PostAsJsonAsync($"api/User/register-user", register);

                registerUserRequest.EnsureSuccessStatusCode();

                return RedirectToAction("Login", "Login");
            }
            return View();
        }

    }
}
