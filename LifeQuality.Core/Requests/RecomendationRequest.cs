﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Requests
{
    public class RecomendationRequest
    {
        public string ReceiverName { get; set; }
        public int AnalysisId { get; set; }
        public string Message { get; set; }
    }
}
