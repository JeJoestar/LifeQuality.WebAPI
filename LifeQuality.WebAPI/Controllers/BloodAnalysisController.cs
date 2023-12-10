using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("blood-analysis")]
    public class BloodAnalysisController : ControllerBase
    {
        private IDataRepository<BloodAnalysisData> _analysisRepository;
        private HangfireService _hangfireService;

        public BloodAnalysisController(IDataRepository<BloodAnalysisData> analysisRepository, HangfireService hangfireService)
        {
            _analysisRepository = analysisRepository;
            _hangfireService = hangfireService;
        }
        [HttpPost]
        public async Task<IActionResult> RequestAnalysis()
        {
            var _analysisToReturn = _analysisRepository.GetAllAsync(orderBy: q => q.OrderByDescending(entity => entity.AnalysisDate));
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateScheduleOfRequests()
        {
            return Ok();
        }
    }
}
