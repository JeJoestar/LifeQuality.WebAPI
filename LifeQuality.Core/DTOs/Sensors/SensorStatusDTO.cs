using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.DTOs.Sensors
{
    public class SensorStatusDTO
    {
        public bool IsAvailable { get; set; }
        public int PatientId { get; set; }
        public string Type { get; set; }
    }
}
