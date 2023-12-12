using LifeQuality.Core.Services;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("blood-analysis")]
    [Authorize]
    public class BloodAnalysisController : ControllerBase
    {
        private BloodAndAnalysisService _bloodAndAnalysisService;
        private HangfireService _hangfireService;

        public BloodAnalysisController(HangfireService hangfireService, BloodAndAnalysisService bloodAndAnalysisService)
        {
            _hangfireService = hangfireService;
            _bloodAndAnalysisService = bloodAndAnalysisService;
        }
        [HttpPost]
        public async Task<IActionResult> RequestAnalysis()
        {
            var _analysisToReturn = _bloodAndAnalysisService.GetAllAsync(orderBy: q => q.OrderByDescending(entity => entity.AnalysisDate));
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateScheduleOfRequests()
        {
            return Ok();
        }
    }
}
