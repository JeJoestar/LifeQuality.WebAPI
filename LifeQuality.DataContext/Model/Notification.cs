using LifeQuality.DataContext.Enums;

namespace LifeQuality.DataContext.Model
{
    public class Notification : EntityBase
    {
        public User Receiver { get; set; }
        public int ReceiverId { get; set; }
        public DateTime Created { get; set; }
        public string RawText { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
