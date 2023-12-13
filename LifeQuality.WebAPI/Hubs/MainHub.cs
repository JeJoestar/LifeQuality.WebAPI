using LifeQuality.Core.DTOs.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace LifeQuality.WebAPI.Hubs
{
    public class MainHub : Hub<IMainHub>
    {
        public async Task SendNotificationToClient(string message)
        {
            await Clients.All.ReceiveNotification(new NotificationDto { Message = message });
        }
    }
}
