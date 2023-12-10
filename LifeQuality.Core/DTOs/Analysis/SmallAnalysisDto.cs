using LifeQuality.DataContext.Model;

namespace LifeQuality.WebAPI.DTOs.Analysis
{
    public class SmallAnalysisDto
    {
        string AnalysisType { get; set; }
        string ReceivedAt { get; set; }
        bool IsRegular { get; set; }
        AnalysisStatus Status { get; set; }
    }
}