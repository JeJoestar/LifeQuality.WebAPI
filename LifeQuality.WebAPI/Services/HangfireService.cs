using Hangfire;
using LifeQuality.Core.Services;

namespace LifeQuality.WebAPI.Services
{
    public class HangfireService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly AnalyticsService _analyticsService;

        public HangfireService(IBackgroundJobClient backgroundJobClient, AnalyticsService analyticsService)
        {
            _backgroundJobClient = backgroundJobClient;
            _analyticsService = analyticsService;
        }

        public string CreateScheduledJob(int sensorId, DateTimeOffset scheduledTime)
        {
            return _backgroundJobClient.Schedule(() => GetScheduledData(sensorId), scheduledTime);
        }

        public async void GetScheduledData(int sensorId)
        {
            await _analyticsService.AnalyseReceivedDataAsync(sensorId);
        }
    }
}
