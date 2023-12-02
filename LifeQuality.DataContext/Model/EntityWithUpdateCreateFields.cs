namespace LifeQuality.DataContext.Model
{
    public class EntityWithUpdateCreateFields : EntityBase
    {
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
