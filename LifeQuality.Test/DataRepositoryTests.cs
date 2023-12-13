using LifeQuality.DataContext;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using System.Linq.Expressions;

namespace LifeQuality.Test
{
    public class DataRepositoryTests<TEntity> where TEntity : EntityBase
    {
        private Mock<IDataContext> _mockContext;
        private Mock<DbSet<TEntity>> _mockDbSet;
        private DataRepository<TEntity> _dataRepository;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<IDataContext>();
            _mockDbSet = new Mock<DbSet<TEntity>>();
            _mockContext.Setup(c => c.Set<TEntity>()).Returns(_mockDbSet.Object);
            _dataRepository = new DataRepository<TEntity>(_mockContext.Object);
        }
        [Test]
        public async Task GetByAsync_ReturnsEntity_WhenFilterMatches()
        {
            TEntity expectedEntity = (TEntity)new EntityBase { Id = 1 };
            _mockDbSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns(expectedEntity);

            var result = await _dataRepository.GetByAsync(e => e.Id == 1);

            Assert.NotNull(result);
        }
        [Test]
        public async Task GetByAsync_ReturnsEntityWhenFound()
        {
            var entityId = 1;
            TEntity entity = (TEntity)new EntityBase() { Id = entityId };
            _mockDbSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns(entity);

            var result = await _dataRepository.GetByAsync(e => e.Id == entityId);

            Assert.AreEqual(entityId, result.Id);
        }
        [Test]
        public async Task GetByAsync_ReturnsNullWhenNotFound()
        {
            var entityId = 1;
            _mockDbSet.Setup(d => d.FirstOrDefault(It.IsAny<Expression<Func<TEntity, bool>>>()))
                      .Returns((TEntity)null);

            var result = await _dataRepository.GetByAsync(e => e.Id == entityId);

            Assert.Null(result);
        }
        [Test]
        public async Task SaveAsync_CallsSaveChangesAsync()
        {
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            await _dataRepository.SaveAsync();

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public async Task AllAsync_ReturnsAllEntities()
        {
            var entities = new List<TEntity>
            {
                (TEntity)new EntityBase() { Id = 1 },
                (TEntity)new EntityBase() { Id = 2 },
                (TEntity)new EntityBase() { Id = 3 }
        };
            _mockDbSet.Setup(d => d.AsNoTracking()).Returns(_mockDbSet.Object);
            _mockDbSet.Setup(d => d.ToList()).Returns(entities);

            var result = await _dataRepository.AllAsync();

            Assert.AreEqual(entities.Count, result.Count);
        }
        [Test]
        public async Task AddNew_AddsNewEntity()
        {
            TEntity entity = (TEntity)new EntityBase();

            _dataRepository.AddNew(entity);

            _mockDbSet.Verify(d => d.Add(entity), Times.Once);
        }

        [Test]
        public async Task Update_UpdatesEntity()
        {
            TEntity entity = (TEntity)new EntityBase();

            _dataRepository.Update(entity);

            _mockDbSet.Verify(d => d.Update(entity), Times.Once);
        }

        [Test]
        public async Task Remove_RemovesEntity()
        {
            var entityId = 1;
            TEntity entity = (TEntity)new EntityBase { Id = entityId };
            _mockDbSet.Setup(d => d.FirstOrDefault(It.IsAny<Expression<Func<EntityBase, bool>>>()))
                      .Returns(entity);

            await _dataRepository.Remove(entityId);

            _mockDbSet.Verify(d => d.Remove(entity), Times.Once);
        }

        [Test]
        public async Task SaveAsync_CallsSaveChanges()
        {
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            await _dataRepository.SaveAsync();

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
        [Test]
        public async Task GetAllAsync_ReturnsEntitiesWithPredicateOrderByAndInclude()
        {
            var entities = new List<TEntity>
            {
                (TEntity)new EntityBase() { Id = 3},
                (TEntity)new EntityBase() { Id = 1},
                (TEntity)new EntityBase() { Id = 2}
            };

            _mockDbSet.Setup(d => d.AsNoTracking()).Returns(_mockDbSet.Object);
            _mockDbSet.Setup(d => d.IgnoreQueryFilters()).Returns(_mockDbSet.Object);
            _mockDbSet.Setup(d => d.Where(It.IsAny<Expression<Func<TEntity, bool>>>())).Returns(_mockDbSet.Object);
            _mockDbSet.Setup(d => d.ToList()).Returns(entities);

            // Act
            var result = await _dataRepository.GetAllAsync(
                predicate: e => e.Id == 1,
                orderBy: q => q.OrderBy(e => e.Id));

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }
    }
}