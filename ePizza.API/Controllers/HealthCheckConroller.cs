using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckConroller : ControllerBase
    {

        [HttpGet]
        [Route("status")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
