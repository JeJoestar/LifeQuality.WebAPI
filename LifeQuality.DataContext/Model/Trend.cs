namespace LifeQuality.DataContext.Model
{
    public class Trend: EntityWithUpdateCreateFields
    {
        public string TrendContent { get; set; }
        public string DateTime { get; set; }
        public string Category { get; set; }
    }
}
