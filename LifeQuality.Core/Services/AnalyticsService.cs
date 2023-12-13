using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;

namespace LifeQuality.Core.Services
{
    public class AnalyticsService
    {
        private readonly IDataRepository<Sensor> _sensorRepository;
        public AnalyticsService(IDataRepository<Sensor> sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
        public async Task<BloodAnalysisData> AnalyseReceivedDataAsync(int sensorId, int patientId, bool isRegular)
        {
            var sensorToRead = await _sensorRepository.GetFirstOrDefaultAsync(x => x.Id == sensorId);
            RemoveAnomalies(sensorToRead);
            AnalysisStatus status;

            if(sensorToRead.ReadingType == DataContext.Enums.ReadingType.Scheduled)
            {
                status = AnalysisStatus.Done;
            }
            else if(sensorToRead.ReadingType == DataContext.Enums.ReadingType.Delayed)
            {
                status = AnalysisStatus.Pending;
            }
            else if (sensorToRead.ReadingType == DataContext.Enums.ReadingType.Manual)
            {
                status = AnalysisStatus.Done;
            }
            else
            {
                status = AnalysisStatus.Failed;
            }

            BloodAnalysisData data = null;

            if (sensorToRead.Type == "ЗАК")
            {
                data = new GeneralBloodAnalysisData()
                {
                    Status = status,
                    WBC = Guid.NewGuid().ToString(),
                    HGB = Guid.NewGuid().ToString(),
                    RBC = Guid.NewGuid().ToString(),
                    HCT = Guid.NewGuid().ToString(),
                    MCV = Guid.NewGuid().ToString(),
                };
            }
            else if (sensorToRead.Type == "Цукор")
            {
                data = new SugarBloodAnalysisData()
                {
                    Status = status,
                    BloodSugarLevel = Random.Shared.NextDouble(),
                    BloodGlucose = Random.Shared.NextDouble(),
                    HbA1c = Random.Shared.NextDouble()
                };
            }
            else if (sensorToRead.Type == "Холестерин")
            {
                data = new CholesterolBloodAnalysisData()
                {
                    Status = status,
                    CholesterolLevel = Random.Shared.NextDouble(),
                    Triglyceride = Random.Shared.NextDouble(),
                    NormalLevelRequirement = Random.Shared.NextDouble()
                };
            }

            if (data is not null)
            {
                data.AnalysisDate = DateTime.UtcNow;
                data.ReceivedAt = DateTime.UtcNow;
                data.IsRegular = isRegular;
                data.SensorId = sensorId;
                data.PatientId = patientId;
                data.CreateData();
            }

            return data;
        }
        public void RemoveAnomalies(Sensor sensorToCheck)
        {
            
        }
    }
}
