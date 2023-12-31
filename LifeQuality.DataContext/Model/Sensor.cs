using LifeQuality.DataContext.Enums;

namespace LifeQuality.DataContext.Model
{
    public class Sensor: EntityWithUpdateCreateFields
    {
        public bool IsAvailable { get; set; }
        public int PatientId { get; set; }
        public string Type { get; set; }
        public ReadingType ReadingType { get; set; }
    }
}
