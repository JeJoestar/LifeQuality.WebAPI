using LifeQuality.DataContext.Model;

namespace LifeQuality.Core.Services
{
    public interface IAnalyticsService
    {
        Task<BloodAnalysisData> AnalyseReceivedDataAsync(int sensorId, int patientId, bool isRegular);
    }
}