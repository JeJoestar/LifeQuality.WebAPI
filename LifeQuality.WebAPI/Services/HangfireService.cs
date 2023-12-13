using Hangfire;
using LifeQuality.Core.Services;

namespace LifeQuality.WebAPI.Services
{
    public class HangfireService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly BloodAndAnalysisService _bloodAndAnalysisService;

        public HangfireService(IBackgroundJobClient backgroundJobClient, BloodAndAnalysisService bloodAndAnalysisService)
        {
            _backgroundJobClient = backgroundJobClient;
            _bloodAndAnalysisService = bloodAndAnalysisService;
        }

        public string CreateRecurrentJobUntil(int sensorId, int patientId, TimeSpan scheduledTime, DateTime? start = null, DateTime? until = null)
        {
            var newScheduledTime = (start ?? DateTime.UtcNow).Add(scheduledTime);
            return _backgroundJobClient.Schedule(() => GetScheduledData(sensorId, patientId, scheduledTime, until), newScheduledTime);
        }

        public async Task GetScheduledData(int sensorId, int patientId, TimeSpan scheduledTime, DateTime? until = null)
        {
            await _bloodAndAnalysisService.CreateAnalysisDataAsync(sensorId, patientId, true);

            if (!until.HasValue)
            {
                _backgroundJobClient.Schedule(() => GetScheduledData(sensorId, patientId, scheduledTime, null), scheduledTime);
            } 
            else if (DateTime.UtcNow.Add(scheduledTime) < until)
            {
                _backgroundJobClient.Schedule(() => GetScheduledData(sensorId, patientId, scheduledTime, until), scheduledTime);
            }

        }
    }
}
