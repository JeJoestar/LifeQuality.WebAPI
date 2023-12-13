using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Requests
{
    public class DelayedAnalysisRequest : AnalysisRequest
    {
        public DateTimeOffset DateTimeOffset { get; set; }
    }
}
