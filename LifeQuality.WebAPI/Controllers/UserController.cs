using AutoMapper;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private IDataRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IDataRepository<User> userRepository) 
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        [HttpGet("GetPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var patientsToReturn = _mapper.Map<IEnumerable<PatientDto>>(await _userRepository.GetAllAsync());
            return Ok(patientsToReturn);
        }
        [HttpGet("GetPatient/{id}")]
        public async Task<IActionResult> GetPatient([FromRoute] int id)
        {
            var patientToReturn = _mapper.Map<PatientDto>(await _userRepository.GetByAsync(p => p.Id == id));
            return Ok(patientToReturn);
        }
        [HttpGet("GetProfile/{id}")]
        public async Task<IActionResult> GetProfile([FromRoute] int id)
        {
            //TODO: new mapper to User
            var userToReturn = _mapper.Map<PatientDto>(await _userRepository.GetByAsync(p => p.Id == id));
            return Ok(userToReturn);
        }
        [HttpPost("CreatePatient")]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto patientCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var patientEntity = _mapper.Map<User>(patientCreateDto);
            _userRepository.AddNew(patientEntity);
            await _userRepository.SaveAsync();
            
            return Ok();
        }
    }
}