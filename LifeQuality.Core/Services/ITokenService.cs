using LifeQuality.DataContext.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Services
{
    public interface ITokenService
    {
        public string GenerateJwtToken(User user);
        public string ValidateJwtToken(string token);
    }
}
