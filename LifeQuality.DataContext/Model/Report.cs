namespace LifeQuality.DataContext.Model
{
    public class Report: EntityWithUpdateCreateFields
    {
        public string ReportContext { get; set; }
        public int AuthorId { get; set; }
        public int AnalisisId { get; set; }
        public BloodAnalysisData Analisis { get; set; }
        public int PatientId { get; set; }
    }
}
