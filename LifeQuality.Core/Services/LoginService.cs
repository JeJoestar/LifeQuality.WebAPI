using LifeQuality.Core.Response;
using LifeQuality.DataContext.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuality.Core.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserProvider _userProvider;

        public LoginService(
            ITokenService tokenService,
            IUserProvider userProvider)
        {
            _tokenService = tokenService;
            _userProvider = userProvider;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(
            string name,
            string password,
            bool useHash = true)
        {
            User user = await _userProvider.GetByNameAsync(name);

            string hash = password;

            if (useHash)
            {
                hash = CryptoHelper.GenerateSaltedHash(password);
            }
            if (user is null || user.Password != hash)
            {
                throw new AuthenticationException("NameOrPasswordIncorrect");
            }

            return await AuthenticateAsync(user);
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(User user)
        {
            string jwtToken = _tokenService.GenerateJwtToken(user);

            await _userProvider.UpdateAsync(user);

            return new(user, jwtToken);
        }
    }
}
