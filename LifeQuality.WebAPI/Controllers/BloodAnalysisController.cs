using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("blood-analysis")]
    public class BloodAnalysisController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RequestAnalysis()
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateScheduleOfRequests()
        {
            return Ok();
        }
    }
}
