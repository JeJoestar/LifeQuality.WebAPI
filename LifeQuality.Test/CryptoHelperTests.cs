using LifeQuality.Core.Services;
using System.Linq.Expressions;

namespace LifeQuality.Test
{
    [TestFixture]
    public class CryptoHelperTests
    {
        [Test]
        public void GenerateSaltedHash_ValidPassword_ReturnsNonEmptyString()
        {
            string password = "MySecurePassword";

            string? result = CryptoHelper.GenerateSaltedHash(password);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GenerateSaltedHash_NullPassword_ReturnsNull()
        {
            string password = null;

            string? result = CryptoHelper.GenerateSaltedHash(password);

            Assert.IsNull(result);
        }

        [Test]
        public void GenerateSaltedHash_EmptyPassword_ReturnsNull()
        {
            string password = string.Empty;

            string? result = CryptoHelper.GenerateSaltedHash(password);

            Assert.IsNull(result);
        }
    }
}