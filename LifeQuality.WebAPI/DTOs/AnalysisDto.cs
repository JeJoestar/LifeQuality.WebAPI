namespace LifeQuality.WebAPI.DTOs
{
    public class AnalysisDto
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string? Type { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string Address { get; set; }
    }
}
