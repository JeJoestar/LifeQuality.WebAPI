using Hangfire;

namespace LifeQuality.WebAPI.Services
{
    public class HangfireService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangfireService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        public void CreateDelayedJob(string jobType)
        {
            _backgroundJobClient.Enqueue(() => YourDelayedJobMethod(jobType));
        }

        public void CreateScheduledJob(string jobType, DateTimeOffset scheduledTime)
        {
            _backgroundJobClient.Schedule(() => YourScheduledJobMethod(jobType), scheduledTime);
        }

        public void CreateManualJob(string jobType)
        {
            _backgroundJobClient.Enqueue(() => YourManualJobMethod(jobType));
        }
        public void YourDelayedJobMethod(string jobType)
        {

        }

        public void YourScheduledJobMethod(string jobType)
        {

        }

        public void YourManualJobMethod(string jobType)
        {

        }
    }
}
