using AutoMapper;
using LifeQuality.Core.DTOs;
using LifeQuality.Core.DTOs.Notifications;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Hubs;
using LifeQuality.Core.Requests;
using LifeQuality.Core.Services;
using LifeQuality.DataContext.Enums;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository<Patient> _patientRepository;
        private readonly IDataRepository<Doctor> _doctorRepository;
        private readonly IDataRepository<Notification> _notificationsRepository;
        private readonly IDataRepository<Recomendation> _recomendationRepository;
        private readonly IHubContext<MainHub> _hubContext;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper,
            IDataRepository<Patient> patientRepository,
            IDataRepository<Doctor> doctorRepository,
            IDataRepository<Notification> notificationsRepository,
            IDataRepository<Recomendation> recomendationRepository,
            IHubContext<MainHub> hubContext) 
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _hubContext = hubContext;
            _recomendationRepository = recomendationRepository;
            _notificationsRepository = notificationsRepository;
        }

        [HttpGet("patients")]
        [ProducesResponseType(typeof(List<PatientDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatients([FromQuery] string? filterByName = null)
        {
            var doctorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doctorId))
            {
                return NotFound();
            }

            var patientsToReturn = _mapper.Map<List<PatientDto>>(
                await _patientRepository.GetByManyAsync(p => 
                (string.IsNullOrEmpty(filterByName) || p.Name.ToLower().Contains(filterByName.ToLower())) 
                && p.DoctorId == Convert.ToInt32(doctorId)));
            
            return Ok(patientsToReturn);
        }

        [HttpGet("patients-autocomplete")]
        [ProducesResponseType(typeof(List<FastEntityDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAutocompletePatients([FromQuery] string? filterByName = null)
        {
            var doctorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doctorId))
            {
                return NotFound();
            }

            var patientsToReturn = _mapper.Map<List<FastEntityDto>>(
                await _patientRepository.GetByManyAsync(p =>
                (string.IsNullOrEmpty(filterByName) || p.Name.ToLower().Contains(filterByName.ToLower()))
                && p.DoctorId == Convert.ToInt32(doctorId)));

            return Ok(patientsToReturn);
        }

        [HttpGet("patient/{id}")]
        [ProducesResponseType(typeof(PatientInfoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatientInfo([FromRoute] int id)
        {
            var patientToReturn = await _patientRepository
                .Include(p => p.BloodAnalysisDatas)
                .Include(p => p.Recomendations)
                .Include(p => p.BloodAnalysisDatas)
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patientToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientInfoDto>(patientToReturn));
        }

        //[HttpGet("recomendation/{id}")]
        //public async Task<IActionResult> GetRecomendation([FromRoute] int id)
        //{
        //    var patientToReturn = await _recomendationRepository
        //        .Include(p => p.Analysis)
        //        .Include(p => p.Analysis.Sensor)
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (patientToReturn == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        //}

        [HttpGet("doctor-profile/{id}")]
        [ProducesResponseType(typeof(DoctorProfileDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorProfile([FromRoute] int id)
        {
            var doctorToReturn = await _doctorRepository.GetFirstOrDefaultAsync(p => p.Id == id);

            if (doctorToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DoctorProfileDto>(doctorToReturn));
        }

        [HttpGet("patient-profile/{id}")]
        [ProducesResponseType(typeof(PatientProfileDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatientProfile([FromRoute] int id)
        {
            var patientToReturn = await _patientRepository
                .Include(p => p.BloodAnalysisDatas)
                    .ThenInclude(b => b.Sensor)
                .Include(p => p.Doctor)
                .Include(p => p.Recomendations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patientToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        }

        [HttpPost("recomendation")]
        public async Task<IActionResult> CreateRecomendation([FromBody]RecomendationRequest recommendationRequest)
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var doctorToReturn = await _doctorRepository.GetFirstOrDefaultAsync(p => p.Id == userId);
            var receiver = await _patientRepository.GetFirstOrDefaultAsync(p => p.Name == recommendationRequest.ReceiverName);

            var rec = new Recomendation()
            {
                RecieverId = receiver.Id,
                AnalysisId = recommendationRequest.AnalysisId,
                Content = recommendationRequest.Message
            };

            _recomendationRepository.AddNew(rec);
            await _recomendationRepository.SaveAsync();

            var notification = new Notification()
            {
                Created = DateTime.Now,
                RawText = "New analysis",
                ReceiverId = receiver.Id,
                NotificationType = NotificationType.NewAnalysis,
            };

            _notificationsRepository.AddNew(notification);
            await _notificationsRepository.SaveAsync();

            await _hubContext.Clients.User(receiver.Id.ToString()).SendAsync("ReceiveNotification", new NotificationDto ()
            {
                Id = notification.Id,
                NotificationType = NotificationType.NewRecommendation,
                Title = "You received a new recommendation",
            });

            return Ok();
        }


        [HttpGet("recomendation/{id}")]
        [ProducesResponseType(typeof(RecommendationDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRecomendation([FromRoute] int id)
        {
            var recommendation = await _recomendationRepository
                .Include(r => r.Reciever)
                    .ThenInclude(p => p.Doctor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recommendation is null)
            {
                return NotFound();
            }

            return Ok(new RecommendationDto()
            {
                DoctorName = recommendation.Reciever.Doctor.Name,
                Message = recommendation.Content,
                ReceivedAt = recommendation.CreatedAt ?? DateTime.UtcNow,
            });
        }

        [HttpPost("patient")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePatient()
        {
            var patient = new Patient()
            {
                Name = "Ivan",
                Password = "Password1",
                PhoneNumber = "1234567890",
                Email = "Email@gmail.com",
                Age = 30,
                UserType = UserType.Patient,
                PatientStatus = PatientStatus.Stable,
                Height = 30,
                Weight = 30,
                BloodType = "q",
                Description = "Sick very sick",
                DoctorId = 1
            };
            _patientRepository.AddNew(patient);
            await _patientRepository.SaveAsync();

            return Ok();
        }


        [HttpPost("doctor")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDoctor()
        {
            var doctor = new Doctor()
            {
                Age = 30,
                CreatedAt = DateTime.UtcNow,
                Email = "doctor@gmail.com",
                Name = "Олег Фрайт",
                Password = "String123@",
                PhoneNumber = "0667192834",
                Speciality = "Лікар-дерматолог",
                UpdatedAt = DateTime.UtcNow,
                UserType = UserType.Doctor,
            };
            _doctorRepository.AddNew(doctor);
            await _doctorRepository.SaveAsync();

            return Ok();
        }

        [HttpGet("notifications")]
        [ProducesResponseType(typeof(List<NotificationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserNotifications()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            List<Notification> notifications = await _notificationsRepository.GetByManyAsync(u => u.ReceiverId == userId) ?? new List<Notification>();

            return Ok(_mapper.Map<List<Notification>, List<NotificationDto>>(notifications));
        }
    }
}