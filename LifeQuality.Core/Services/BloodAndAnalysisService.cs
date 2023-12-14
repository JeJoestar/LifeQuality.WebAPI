using LifeQuality.Core.DTOs.Notifications;
using LifeQuality.Core.Hubs;
using LifeQuality.DataContext.Enums;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.AspNetCore.SignalR;
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
        private IDataRepository<Notification> _notificationsRepository;
        private IDataRepository<Patient> _patientsRepository;
        private IHubContext<MainHub> _hubContext;

        public BloodAndAnalysisService(
            AnalyticsService analyticsService,
            IDataRepository<BloodAnalysisData> analysisRepository,
            IDataRepository<Notification> notificationsRepository,
            IDataRepository<Patient> patientsRepository,
            SensorClient sensorClient,
            IHubContext<MainHub> hubContext)
        {
            _analyticsService = analyticsService;
            _analysisRepository = analysisRepository;
            _sensorClient = sensorClient;
            _hubContext = hubContext;
            _patientsRepository = patientsRepository;
            _notificationsRepository = notificationsRepository;
        }
        public async Task CreateAnalysisDataAsync(int sensorId, int patientId, bool isRegular)
        {
            var readenData = await _analyticsService.AnalyseReceivedDataAsync(sensorId, patientId, isRegular);
            _analysisRepository.AddNew(readenData);
            await _analysisRepository.SaveAsync();

            var patient = await _patientsRepository.GetByAsync(p => p.Id == patientId);
            var notification = new Notification()
            {
                Created = DateTime.UtcNow,
                RawText = "New analysis",
                ReceiverId = patient.DoctorId,
                NotificationType = NotificationType.NewAnalysis,
            };

            _notificationsRepository.AddNew(notification);
            await _notificationsRepository.SaveAsync();

            await _hubContext.Clients.User(patient.DoctorId.ToString()).SendAsync("ReceiveNotification", new NotificationDto()
            {
                Id = notification.Id,
                NotificationType = NotificationType.NewAnalysis,
                Title = notification.RawText,
            });
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
