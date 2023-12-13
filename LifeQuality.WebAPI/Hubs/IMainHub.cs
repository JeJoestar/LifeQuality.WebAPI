using LifeQuality.Core.DTOs.Notifications;

namespace LifeQuality.WebAPI.Hubs
{
    public interface IMainHub
    {
        Task ReceiveNotification(RecommendationDto notification);
    }
}
