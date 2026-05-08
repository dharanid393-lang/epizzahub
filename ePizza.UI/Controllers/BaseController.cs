using ePizza.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ePizza.UI.Controllers
{
    public class BaseController : Controller
    {
        public UserViewModel? CurrentUser
        {

            get
            {
                if (User.Claims.Any())
                {
                    string userName = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
                    string email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
                    string userId = User.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value;

                    return new UserViewModel
                    {
                        Email = email,
                        Name = userName,
                        UserId =Convert.ToInt32(userId)
                    };
                }
                return null;
            }
        }
    }
}
