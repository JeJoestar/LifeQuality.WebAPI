using AutoMapper;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Requests;
using LifeQuality.Core.Services;
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
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository<Patient> _patientRepository;
        private readonly IDataRepository<Doctor> _doctorRepository;
        private readonly IDataRepository<Recomendation> _recomendationRepository;
        private readonly IHubContext<MainHub> _hubContext;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper,
            IDataRepository<Patient> patientRepository,
            IDataRepository<Doctor> doctorRepository,
            IDataRepository<User> userRepository,
            IDataRepository<Recomendation> recomendationRepository,
            IHubContext<MainHub> hubContext) 
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _hubContext = hubContext;
            _recomendationRepository = recomendationRepository;
        }

        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var doctorId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doctorId))
            {
                return NotFound();
            }

            var patientsToReturn = _mapper.Map<IEnumerable<PatientDto>>(await _patientRepository.GetByManyAsync(p => p.DoctorId == Convert.ToInt32(doctorId)));
            
            return Ok(patientsToReturn);
        }
        [HttpGet("GetPatientInfo/{id}")]
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

            return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        }
        [HttpGet("GetRecomendation/{id}")]
        public async Task<IActionResult> GetRecomendation([FromRoute] int id)
        {
            var patientToReturn = await _recomendationRepository
                .Include(p => p.Analysis)
                .Include(p => p.Analysis.Sensor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patientToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        }
        [HttpGet("GetDoctorProfile/{id}")]
        public async Task<IActionResult> GetDoctorProfile([FromRoute] int id)
        {
            var doctorToReturn = await _doctorRepository.GetFirstOrDefaultAsync(p => p.Id == id);

            if (doctorToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DoctorProfileDto>(doctorToReturn));
        }
        [HttpGet("GetPatientProfile/{id}")]
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
        [HttpPost("CreateRecomendations")]
        public async Task<IActionResult> CreateRecomendations([FromBody]RecomendationRequest recomendationRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await _hubContext.Clients.User(userId).SendAsync(recomendationRequest.Message);

            var rec = new Recomendation()
            {
                RecieverId = recomendationRequest.UserId,
                Content = recomendationRequest.Message
            };

            _recomendationRepository.AddNew(rec);
            await _recomendationRepository.SaveAsync();

            return Ok();
        }
        [HttpGet("CreatePatient")]
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
                PatientStatus = DataContext.Enums.PatientStatus.Sick,
                Height = 30,
                Weight = 30,
                BloodType = "q",
                Description = "Sick very sick",
                DoctorId = 1
            };
            _patientRepository.AddNew(patient);
            _patientRepository.SaveAsync();
            Console.WriteLine("qwe");
            return Ok();
        }
    }
}