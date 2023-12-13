using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.DTOs.Notifications
{
    public class RecommendationDto
    {
        public string DoctorName { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string Message { get; set; }
    }
}
