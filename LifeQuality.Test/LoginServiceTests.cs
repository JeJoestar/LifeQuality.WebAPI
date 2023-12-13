using LifeQuality.Core.Services;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
using Moq;

namespace LifeQuality.Test
{
    [TestFixture]
    public class LoginServiceTests
    {
        [Test]
        public async Task AuthenticateAsync_User_ReturnsAuthenticateResponse()
        {
            var tokenServiceMock = new Mock<ITokenService>();
            var userRepositoryMock = new Mock<IDataRepository<User>>();

            var service = new LoginService(tokenServiceMock.Object, userRepositoryMock.Object);

            var user = new User
            {
                Name = "testUser",
                Password = "hashedPassword"
            };

            tokenServiceMock.Setup(t => t.GenerateJwtToken(user)).Returns("jwtToken");

            var result = await service.AuthenticateAsync(user);

            Assert.IsNotNull(result);
        }
    }
}