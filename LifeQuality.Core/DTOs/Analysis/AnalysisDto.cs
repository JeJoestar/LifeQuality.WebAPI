using LifeQuality.DataContext.Model;

namespace LifeQuality.WebAPI.DTOs.Analysis
{
    public class AnalysisDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string? Type { get; set; }
        public DateTime ReceivedAt { get; set; }
        public AnalysisStatus Status { get; set; }
    }
}