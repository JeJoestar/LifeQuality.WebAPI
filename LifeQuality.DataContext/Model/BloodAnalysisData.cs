using LifeQuality.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.DataContext.Model
{
    public abstract class BloodAnalysisData: EntityBase
    {
        public string AnalysisDate { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public DateTime ReceivedAt { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
        public AnalysisStatus Status { get; set; }
        public abstract void CreateData();
    }
}
