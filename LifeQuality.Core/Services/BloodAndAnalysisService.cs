using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LifeQuality.Core.Services
{
    public class BloodAndAnalysisService
    {
        private readonly AnalyticsService _analyticsService;
        private readonly SensorClient _sensorClient;

        private IDataRepository<BloodAnalysisData> _analysisRepository;

        public BloodAndAnalysisService(AnalyticsService analyticsService,

            IDataRepository<BloodAnalysisData> analysisRepository, SensorClient sensorClient)
        {
            _analyticsService = analyticsService;
            _analysisRepository = analysisRepository;
            _sensorClient = sensorClient;
        }
        public async void CreateAnalysisDataAsync(int sensorId)
        {
            var readenData = await _analyticsService.AnalyseReceivedDataAsync(sensorId);
            _analysisRepository.AddNew(readenData);
            await _analysisRepository.SaveAsync();
        }
        public async Task<List<BloodAnalysisData>> GetAllAsync(
            Expression<Func<BloodAnalysisData, bool>> predicate = null,
            Func<IQueryable<BloodAnalysisData>, IOrderedQueryable<BloodAnalysisData>> orderBy = null,
            Func<IQueryable<BloodAnalysisData>, IIncludableQueryable<BloodAnalysisData, object>> include = null,
            bool disableTracking = true,
            bool ignoreQueryFilters = false)
        {
            return await _analysisRepository.GetAllAsync(predicate, orderBy, include, disableTracking, ignoreQueryFilters);
        }
        public IIncludableQueryable<BloodAnalysisData, TNavigateEntity> Include<TNavigateEntity>(Expression<Func<BloodAnalysisData, TNavigateEntity>> navigationPropertyPath)
        {
            return _analysisRepository.Include(navigationPropertyPath);
        }

    }
}
