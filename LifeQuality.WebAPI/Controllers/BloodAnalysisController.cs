using AutoMapper;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Requests;
using LifeQuality.Core.Services;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.DTOs.Analysis;
using LifeQuality.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        private readonly IMapper _mapper;

        public BloodAnalysisController(HangfireService hangfireService,
            BloodAndAnalysisService bloodAndAnalysisService,
            SensorClient sensorClient,
            IMapper mapper)
        {
            _hangfireService = hangfireService;
            _bloodAndAnalysisService = bloodAndAnalysisService;
            _sensorClient = sensorClient;
            _mapper = mapper;
        }
        [HttpGet("by-doctor/{doctorId}")]
        [ProducesResponseType(typeof(List<SmallAnalysisDto>), StatusCodes.Status200OK)]
        public IActionResult RequestAllAnalysis([FromRoute] int doctorId)
        {
            var _analysisToReturn = _mapper.Map<List<SmallAnalysisDto>>(
                _bloodAndAnalysisService
                    .Include(q => q.Sensor)
                    .Include(q => q.Patient)
                    .Where(a => a.Patient.DoctorId == doctorId)
                    .Select(q => q));

            return Ok(_analysisToReturn);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnalysisDto), StatusCodes.Status200OK)]
        public IActionResult GetAnalysisById([FromRoute] int id)
        {
            var _analysisToReturn = _mapper.Map<AnalysisDto>(_bloodAndAnalysisService
                .Include(q => q.Sensor)
                .Include(q => q.Patient)
                .FirstOrDefault(q => q.Id == id));

            return Ok(_analysisToReturn);
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
