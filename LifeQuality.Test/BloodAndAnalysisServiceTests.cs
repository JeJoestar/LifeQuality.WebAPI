//using LifeQuality.Core.Services;
//using LifeQuality.DataContext.Model;
//using LifeQuality.DataContext.Repository;
//using Moq;
//using System.Linq.Expressions;

//namespace LifeQuality.Test
//{
//    [TestFixture]
//    public class BloodAndAnalysisServiceTests
//    {
//        [Test]
//        public async Task CreateAnalysisDataAsync_ValidSensorId_AnalyticsServiceCalled()
//        {
//            var analyticsServiceMock = new Mock<AnalyticsService>();
//            var analysisRepositoryMock = new Mock<IDataRepository<BloodAnalysisData>>();
//            var sensorClientMock = new Mock<SensorClient>();

//            var service = new BloodAndAnalysisService(analyticsServiceMock.Object, analysisRepositoryMock.Object, sensorClientMock.Object);

//            int sensorId = 1;
//            var analyzedData = new GeneralBloodAnalysisData();
//            analyticsServiceMock.Setup(s => s.AnalyseReceivedDataAsync(sensorId, 1, true)).ReturnsAsync(analyzedData);

//            await service.CreateAnalysisDataAsync(sensorId, 1, true);

//            analyticsServiceMock.Verify(s => s.AnalyseReceivedDataAsync(sensorId, 1, true), Times.Once);
//            analysisRepositoryMock.Verify(r => r.AddNew(analyzedData), Times.Once);
//            analysisRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
//        }
//        [Test]
//        public async Task GetAllAsync_WithPredicate_ReturnsFilteredData()
//        {
//            var analysisRepositoryMock = new Mock<IDataRepository<BloodAnalysisData>>();
//            var service = new BloodAndAnalysisService(null, analysisRepositoryMock.Object, null);

//            var data = new List<BloodAnalysisData>
//        {
//            new CholesterolBloodAnalysisData { Triglyceride = 60 },
//            new CholesterolBloodAnalysisData { Triglyceride = 40 },
//        }.AsQueryable();

//            foreach(var el in data)
//            {
//                el.CreateData();
//            }

//            analysisRepositoryMock.Setup(r => r.GetAllAsync(null, null, null, true, false)).ReturnsAsync(data.ToList());

//            var result = await service.GetAllAsync();

//            Assert.IsNotNull(result);
//            Assert.AreEqual(2, result.Count);
//        }
//    }
//}