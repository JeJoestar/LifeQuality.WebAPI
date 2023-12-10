using LifeQuality.DataContext.Enums;
using LifeQuality.WebAPI.DTOs.Analysis;

namespace LifeQuality.WebAPI.DTOs.Users
{
    public class PatientInfoDto
    {
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public string PatientStatusDescription { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public SmallAnalysisDto[] Analysis { get; set; }
    }
}
