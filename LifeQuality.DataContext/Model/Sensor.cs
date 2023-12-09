using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public class Sensor: EntityWithUpdateCreateFields
    {
        public bool IsAvailable { get; set; }
        public int PatientId { get; set; }
        public string Type { get; set; }
    }
}
