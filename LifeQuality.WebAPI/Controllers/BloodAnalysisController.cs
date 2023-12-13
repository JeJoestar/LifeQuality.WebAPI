using AutoMapper;
using LifeQuality.Core.DTOs.Analysis;
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
        private readonly IDataRepository<Patient> _patientRepository;
        private readonly HangfireService _hangfireService;
        private readonly IMapper _mapper;

        public BloodAnalysisController(
            HangfireService hangfireService,
            IDataRepository<Patient> patientRepository,
            BloodAndAnalysisService bloodAndAnalysisService,
            SensorClient sensorClient,
            IMapper mapper)
        {
            _hangfireService = hangfireService;
            _patientRepository = patientRepository;
            _bloodAndAnalysisService = bloodAndAnalysisService;
            _sensorClient = sensorClient;
            _mapper = mapper;
        }
        [HttpGet("by-doctor/{doctorId}")]
        [ProducesResponseType(typeof(List<SmallAnalysisDto>), StatusCodes.Status200OK)]
        public IActionResult RequestAllAnalysis([FromRoute] int doctorId)
        {
            var analysis = _bloodAndAnalysisService
                    .Include(q => q.Sensor)
                    .Include(q => q.Patient)
                    .Where(a => a.Patient.DoctorId == doctorId)
                    .Select(q => q)
                    .ToList();
            var _analysisToReturn = _mapper.Map<List<SmallAnalysisDto>>(analysis);

            return Ok(_analysisToReturn);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AnalysisInfoDto), StatusCodes.Status200OK)]
        public IActionResult GetAnalysisById([FromRoute] int id)
        {
            var _analysisToReturn = _mapper.Map<AnalysisInfoDto>(_bloodAndAnalysisService
                .Include(q => q.Sensor)
                .Include(q => q.Patient)
                .FirstOrDefault(q => q.Id == id));

            return Ok(_analysisToReturn);
        }
        [HttpPost("CreateScheduleOfRequest")]
        public async Task<IActionResult> CreateScheduleOfRequest([FromBody] ScheduledAnalysisRequest analysisRequest)
        {
            var patient = await _patientRepository.GetByAsync(p => p.Name == analysisRequest.PatientName);
            var sensor = await _sensorClient.RequestSensor(patient.Id, analysisRequest.AnalysisType);
            TimeSpan timeSpan = analysisRequest.Interval switch
            {
                IntervalType.Minutes => TimeSpan.FromMinutes(1),
                IntervalType.Hourly => TimeSpan.FromHours(1),
                IntervalType.Daily => TimeSpan.FromDays(1),
                IntervalType.Weekly => TimeSpan.FromDays(7),
                IntervalType.Monthly => TimeSpan.FromDays(30),
            };
            sensor.ReadingType = DataContext.Enums.ReadingType.Scheduled;

            _hangfireService.CreateRecurrentJobUntil(
                sensor.Id,
                patient.Id,
                timeSpan, 
                analysisRequest.StartDate, 
                analysisRequest.EndDate);

            return Ok();
        }

        [HttpPost("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] AnalysisRequest analysisRequest)
        {
            var patient = await _patientRepository.GetByAsync(p => p.Name == analysisRequest.PatientName);
            var sensor = await _sensorClient.RequestSensor(patient.Id, analysisRequest.AnalysisType);

            await _bloodAndAnalysisService.CreateAnalysisDataAsync(sensor.Id, patient.Id, false);

            return Ok();
        }
    }
}
