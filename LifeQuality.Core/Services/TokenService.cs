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
    public class TokenService : ITokenService
    {
        public string GenerateJwtToken(User user)
        {
            List<Claim> claims = new()
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            return GetToken(claims);
        }
        private string GetToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Constants.salt);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Constants.jwtIssuer,
                Audience = Constants.jwtAudience,
                Subject = new(claims),
                Expires = DateTime.UtcNow.AddMinutes(Constants.jwtLifetimeMin),
                SigningCredentials = credentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string ValidateJwtToken(string token)
        {
            if (token is null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Constants.salt;
            try
            {
                tokenHandler.ValidateToken(
                    token,
                    new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "Id").Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}