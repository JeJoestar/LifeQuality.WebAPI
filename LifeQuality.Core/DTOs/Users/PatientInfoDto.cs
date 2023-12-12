using LifeQuality.DataContext.Enums;
using LifeQuality.WebAPI.DTOs.Analysis;

namespace LifeQuality.Core.DTOs.Users
{
    public class PatientInfoDto : UserDto
    {
        public string DoctorName { get; set; }
        public PatientStatus PatientStatus { get; set; }
        public string PatientStatusDescription { get; set; }
        public List<SmallAnalysisDto> Analysis { get; set; }
    }
}
