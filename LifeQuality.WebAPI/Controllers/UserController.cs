using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private IDataRepository<User> _userRepository;
        public UserController(IDataRepository<User> userRepository) 
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patientsToReturn = await _userRepository.GetAllAsync();
            return Ok(patientsToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient([FromRoute] int id)
        {
            var patientToReturn = await _userRepository.GetByAsync(p => p.Id == id);
            return Ok(patientToReturn);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto patientCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var patientEntity = new Patient
            {
                Name = patientCreateDto.Name,
                Email = patientCreateDto.Email,
                PhoneNumber = patientCreateDto.Phone,
                AdditioanlInfo = patientCreateDto.Address,
                Password = "",
                Age = 18,
            };
            _userRepository.AddNew(patientEntity);
            
            return Ok();
        }
    }
}
