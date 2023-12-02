using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient([FromRoute] int id)
        {
            return Ok();
        }
    }
}
