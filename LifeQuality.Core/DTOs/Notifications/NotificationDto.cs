using LifeQuality.DataContext.Enums;

namespace LifeQuality.Core.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
