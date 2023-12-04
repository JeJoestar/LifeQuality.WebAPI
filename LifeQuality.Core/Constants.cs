using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core
{
    public static class Constants
    {
        public readonly static byte[] salt = Encoding.UTF8.GetBytes("PIDORAS");
        public readonly static int jwtLifetimeMin = 3;
        public readonly static string jwtIssuer = "";
        public readonly static string jwtAudience = "";
    }
}
