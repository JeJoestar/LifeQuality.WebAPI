﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Requests
{
    public class AnalysisRequest
    {
        public string PatientName { get; set; }
        public string AnalysisType { get; set;}
        public string? Comment { get; set;}
    }
}
