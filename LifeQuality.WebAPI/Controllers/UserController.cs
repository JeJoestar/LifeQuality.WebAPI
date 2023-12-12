using AutoMapper;
using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Services;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository<User> _userRepository;
        private readonly IDataRepository<Patient> _patientRepository;
        private readonly IDataRepository<Doctor> _doctorRepository;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IDataRepository<User> userRepository,
            IDataRepository<Patient> patientRepository,
            IDataRepository<Doctor> doctorRepository) 
        {
            _mapper = mapper;
            _userRepository = userRepository;
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

            var patientsToReturn = _mapper.Map<IEnumerable<PatientInfoDto>>(await _userRepository.GetByManyAsync(p => p.Id == Convert.ToInt32(doctorId)));
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
            var userToReturn = await _userRepository.GetFirstOrDefaultAsync(p => p.Id == id);

            if (userToReturn == null)
            {
                return NotFound();
            }

            UserDto profileToReturn = null;

            if (userToReturn is Doctor)
            {
                return Ok(_mapper.Map<DoctorProfileDto>(userToReturn as Doctor));
            }
            else if (userToReturn is Patient)
            {
                var patient = userToReturn as Patient;
                return Ok(_mapper.Map<PatientProfileDto>(await _userRepository.Entry(patient));
            }
            else
            {
                return NotFound();
            }
        }
    }
}