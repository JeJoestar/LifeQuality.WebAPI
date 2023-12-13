using LifeQuality.Core.Response;
using LifeQuality.DataContext.Model;
using LifeQuality.DataContext.Repository;
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
        private IDataRepository<User> _userRepository;

        public LoginService(
            ITokenService tokenService,
            IDataRepository<User> userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<AuthenticateResponse> AuthenticateAsync(
            string name,
            string password,
            bool useHash = true)
        {
            User user = await _userRepository.GetByAsync(u=>u.Name ==  name);

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

            _userRepository.Update(user);

            return new(user, jwtToken);
        }
    }
}
