using System.Security.Cryptography;
using System.Text;

namespace LifeQuality.Core.Services
{
    public static class CryptoHelper
    {
        public static string? GenerateSaltedHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            byte[] saltBytes = Constants.salt;

            Rfc2898DeriveBytes rfc2898DeriveBytes = new(password, saltBytes, 10000, HashAlgorithmName.SHA256);
            string hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            return hashPassword;
        }
    }
}