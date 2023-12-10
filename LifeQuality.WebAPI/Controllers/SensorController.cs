using AutoMapper;
using LifeQuality.Core.DTOs.Sensors;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using LifeQuality.WebAPI.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("sensor")]
    public class SensorController : ControllerBase
    {
        private IDataRepository<Sensor> _sensorRepository;
        private readonly IMapper _mapper;
        public SensorController(IDataRepository<Sensor> sensorRepository, IMapper mapper)
        {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }
        [HttpGet("GetSensors")]
        public async Task<IActionResult> GetSensors()
        {
            var sensorsToReturn = await _sensorRepository.GetAllAsync();
            return Ok(sensorsToReturn);
        }
        [HttpGet("CheckStatus/{id}")]
        public async Task<IActionResult> CheckStatus([FromRoute] int id)
        {
            var sensorToReturn = _mapper.Map<SensorStatusDTO>(await _sensorRepository.GetByAsync(s=>s.Id == id));
            return Ok(sensorToReturn);
        }
    }
}
