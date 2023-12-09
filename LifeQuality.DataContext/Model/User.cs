namespace LifeQuality.DataContext.Model
{
    public class User: EntityWithUpdateCreateFields
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
