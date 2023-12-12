using LifeQuality.Core.DTOs.Recommendations;

namespace LifeQuality.Core.DTOs.Users
{
    public class PatientProfileDto : PatientInfoDto
    {
        public List<ShortRecommendationDto> Recommendations { get; set; }
    }
}
