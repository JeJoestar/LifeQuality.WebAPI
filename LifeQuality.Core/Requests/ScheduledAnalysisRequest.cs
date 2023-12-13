using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Requests
{
    public class ScheduledAnalysisRequest: AnalysisRequest
    {
        public TimeSpan TimeSpan { get; set; }
    }
}
