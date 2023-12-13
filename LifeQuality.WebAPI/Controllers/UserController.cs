using AutoMapper;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Services;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        public UserController(IMapper mapper,
            IDataRepository<Patient> patientRepository,
            IDataRepository<Doctor> doctorRepository) 
        {
            _mapper = mapper;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
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
        [HttpGet("GetProfile/{id}")]
        public async Task<IActionResult> GetDoctorProfile([FromRoute] int id)
        {
            var doctorToReturn = await _doctorRepository.GetFirstOrDefaultAsync(p => p.Id == id);

            if (doctorToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DoctorProfileDto>(doctorToReturn));
        }
        [HttpGet("GetProfile/{id}")]
        public async Task<IActionResult> GetPatientProfile([FromRoute] int id)
        {
            var patientToReturn = await _patientRepository.Include(p => p.BloodAnalysisDatas)
                .Include(p => p.Reports)
                .Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == id);

            if (patientToReturn == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PatientProfileDto>(patientToReturn));
        }
        [HttpPost("CreateRecomendations")]
        public async Task<IActionResult> CreateRecomendations()
        {
            return Ok();
        }
    }
}