using LifeQuality.DataContext.Model;

namespace LifeQuality.WebAPI.DTOs.Analysis
{
    public class SmallAnalysisDto
    {
        public string AnalysisType { get; set; }
        public DateTime ReceivedAt { get; set; }
        public bool IsRegular { get; set; }
        public AnalysisStatus Status { get; set; }
    }
}