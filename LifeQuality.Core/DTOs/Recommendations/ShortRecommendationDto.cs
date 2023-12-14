using LifeQuality.WebAPI.DTOs.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.DTOs.Recommendations
{
    public class ShortRecommendationDto
    {
        public int Id { get; set; }
        public SmallAnalysisDto Analysis { get; set; }
        public string ReceivedAt { get; set; }
        public string Content { get; set; }
    }
}
