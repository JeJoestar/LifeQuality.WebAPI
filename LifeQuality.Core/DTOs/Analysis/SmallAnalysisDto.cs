using LifeQuality.DataContext.Model;

namespace LifeQuality.WebAPI.DTOs.Analysis
{
    public class SmallAnalysisDto
    {
        public int Id { get; set; }
        public string AnalysisType { get; set; }
        public string PatientName { get; set; }
        public DateTime ReceivedAt { get; set; }
        public bool IsRegular { get; set; }
        public AnalysisStatus Status { get; set; }
    }
}