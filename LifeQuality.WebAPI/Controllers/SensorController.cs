using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("sensor")]
    public class SensorController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> CheckStatus()
        {
            return Ok();
        }
    }
}
