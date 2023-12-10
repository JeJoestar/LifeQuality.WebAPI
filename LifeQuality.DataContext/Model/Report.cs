namespace LifeQuality.DataContext.Model
{
    public class Report: EntityWithUpdateCreateFields
    {
        public string CreatedAt { get; set; }
        public string ReportContext { get; set; }
        public int AuthorId { get; set; }
        public int AnalisisId { get; set; }
        public int PatientId { get; set; }
    }
}
