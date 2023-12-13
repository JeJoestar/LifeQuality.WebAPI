using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core
{
    public static class Constants
    {
        public readonly static byte[] salt = Encoding.UTF8.GetBytes("VeryGoodSaltForTokens");
        public readonly static int jwtLifetimeMin = 30;
        public readonly static string jwtIssuer = "QualityLife";
        public readonly static string jwtAudience = "QualityLifeClient";
        public static string BearerAuth { get; } = "bearerAuth";
    }
}
