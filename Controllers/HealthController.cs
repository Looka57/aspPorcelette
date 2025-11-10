using Microsoft.AspNetCore.Mvc;

namespace ASPPorcelette.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "Healthy", time = System.DateTime.UtcNow });
    }
}
