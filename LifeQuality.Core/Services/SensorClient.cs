using LifeQuality.DataContext.Enums;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Services
{
    public class SensorClient
    {
        private IDataRepository<Sensor> _sensorRepository;

        public SensorClient(IDataRepository<Sensor> sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
        public SensorClient()
        {

        }

        public async Task<Sensor> RequestSensor(int patientId, string type)
        {
            var sensor = new Sensor()
            {
                IsAvailable = true,
                PatientId = patientId,
                Type = type,
                ReadingType = ReadingType.Manual
            };

            _sensorRepository.AddNew(sensor);
            await _sensorRepository.SaveAsync();

            return sensor;
        }
    }
}
