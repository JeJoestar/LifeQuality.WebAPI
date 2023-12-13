using LifeQuality.Core.Requests;
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
        private readonly BloodAndAnalysisService _bloodAndAnalysisService;
        private readonly SensorClient _sensorClient;
        private readonly HangfireService _hangfireService;

        public BloodAnalysisController(HangfireService hangfireService,
            BloodAndAnalysisService bloodAndAnalysisService,
            SensorClient sensorClient)
        {
            _hangfireService = hangfireService;
            _bloodAndAnalysisService = bloodAndAnalysisService;
            _sensorClient = sensorClient;
        }
        [HttpGet("RequestAnalysis")]
        public async Task<IActionResult> RequestAnalysis()
        {
            var _analysisToReturn = _bloodAndAnalysisService.
                GetAllAsync(orderBy: q => q.OrderByDescending(entity => entity.AnalysisDate));
            return Ok();
        }
        [HttpPost("CreateScheduleOfRequest")]
        public async Task<IActionResult> CreateScheduleOfRequest([FromBody] ScheduledAnalysisRequest analysisRequest)
        {
            var sensor = await _sensorClient.RequestSensor(analysisRequest.PatientId, analysisRequest.AnalysisType);

            _hangfireService.CreateScheduledJob(sensor.Id, analysisRequest.TimeSpan);

            return Ok();
        }
        [HttpPost("CreateDelayedRequest")]
        public async Task<IActionResult> CreateDelayedRequest([FromBody] DelayedAnalysisRequest analysisRequest)
        {
            var sensor = await _sensorClient.RequestSensor(analysisRequest.PatientId, analysisRequest.AnalysisType);

            _hangfireService.CreateDelayedJob(sensor.Id, analysisRequest.DateTimeOffset);

            return Ok();
        }
        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] AnalysisRequest analysisRequest)
        {
            var sensor = await _sensorClient.RequestSensor(analysisRequest.PatientId, analysisRequest.AnalysisType);

            _bloodAndAnalysisService.CreateAnalysisDataAsync(sensor.Id);

            return Ok();
        }
    }
}
