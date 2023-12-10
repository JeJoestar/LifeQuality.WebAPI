using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;

namespace LifeQuality.Core.Services
{
    public class AnalysisService
    {
        private readonly IDataRepository<Sensor> _sensorRepository;
        public AnalysisService(IDataRepository<Sensor> sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }
        public async Task<BloodAnalysisData> AnalyseReceivedData(int id)
        {
            var sensorToRead = await _sensorRepository.GetByAsync(s => s.Id == id);
            RemoveAnomalies();
            if (sensorToRead.Type == "General") 
            {
                return new GeneralBloodAnalysisData()
                {
                    WBC = Guid.NewGuid().ToString(),
                    HGB = Guid.NewGuid().ToString(),
                    RBC = Guid.NewGuid().ToString(),
                    HCT = Guid.NewGuid().ToString(),
                    MCV = Guid.NewGuid().ToString(),
                };
            }
            else if(sensorToRead.Type == "Sugar")
            {
                return new SugarBloodAnalysisData()
                {
                    BloodSugarLevel = Random.Shared.NextDouble(),
                    BloodGlucose = Random.Shared.NextDouble(),
                    HbA1c = Random.Shared.NextDouble()
                };
            }
            else if(sensorToRead.Type == "Cholesterol")
            {
                return new CholesterolBloodAnalysisData()
                {
                    CholesterolLevel = Random.Shared.NextDouble(),
                    Triglyceride = Random.Shared.NextDouble(),
                    NormalLevelRequirement = Random.Shared.NextDouble()
                };
            }
            return null;
        }
        private void RemoveAnomalies()
        {

        }
    }
}
