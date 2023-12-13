using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Requests
{
    public class ScheduledAnalysisRequest: AnalysisRequest
    {
        public IntervalType Interval { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public enum IntervalType
    {
        Minutes,
        Hourly,
        Daily,
        Weekly,
        Monthly,
    }
}
