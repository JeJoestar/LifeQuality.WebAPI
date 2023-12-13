using AutoMapper;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Requests;
using LifeQuality.Core.Services;
using LifeQuality.DataContext.Enums;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.Hubs;
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
        private readonly IDataRepository<Recomendation> _recomendationRepository;
        private readonly IHubContext<MainHub, IMainHub> _hubContext;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper,
            IDataRepository<Patient> patientRepository,
            IDataRepository<Doctor> doctorRepository,
            IDataRepository<User> userRepository,
            IDataRepository<Recomendation> recomendationRepository,
            IHubContext<MainHub, IMainHub> hubContext) 
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _hubContext = hubContext;
            _recomendationRepository = recomendationRepository;
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
                .Include(p => p.Doctor)
                .Include(p => p.BloodAnalysisDatas)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patientToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        }

        [HttpPost("recomendation")]
        public async Task<IActionResult> CreateRecomendation([FromBody]RecomendationRequest recomendationRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            var rec = new Recomendation()
            {
                RecieverId = recomendationRequest.UserId,
                Content = recomendationRequest.Message
            };

            _recomendationRepository.AddNew(rec);
            await _recomendationRepository.SaveAsync();

            await _hubContext.Clients.User(userId).ReceiveNotification(new()
            {
                Message = recomendationRequest.Message
            });

            return Ok();
        }

        [HttpPost("patient")]
        public async Task<IActionResult> CreatePatient()
        {
            var patient = new Patient()
            {
                Name = "Ivan",
                Password = "Password",
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
        public async Task<IActionResult> CreateDoctor()
        {
            var doctor = new Doctor()
            {
                Id = 1,
                Age = 30,
                CreatedAt = DateTime.UtcNow,
                Email = "doctor@gmail.com",
                Name = "Олег Фрайт",
                Password = "qwerty1",
                PhoneNumber = "0667192834",
                Speciality = "Лікар-дерматолог",
                UpdatedAt = DateTime.UtcNow,
                UserType = UserType.Doctor,
            };
            _doctorRepository.AddNew(doctor);
            await _doctorRepository.SaveAsync();

            return Ok();
        }
    }
}