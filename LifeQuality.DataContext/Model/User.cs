namespace LifeQuality.DataContext.Model
{
    public class User: EntityWithUpdateCreateFields
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int Age { get; set; }
        public UserType UserType { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
