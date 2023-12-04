using LifeQuality.Core.Response;
using LifeQuality.DataContext.Model;

namespace LifeQuality.Core.Services
{
    public interface ILoginService
    {
        Task<AuthenticateResponse> AuthenticateAsync(string mail, string password, bool useHash = true);

        Task<AuthenticateResponse> AuthenticateAsync(User user);
    }
}