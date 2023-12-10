using LifeQuality.DataContext.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.DTOs.Analysis
{
    public class AnalysisInfoDto
    {
        public string Type { get; set; }
        public string PatientName { get; set; }
        public string DateTime { get; set; }
        public AnalysisStatus Status { get; set; }
        public string Description { get; set; }
        public string[] Files { get; set; }
    }
}
