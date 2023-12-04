using LifeQuality.DataContext.Model;

namespace LifeQuality.Core.Services
{
    public interface IUserProvider
    {
        Task UpdateAsync(User user);
        Task InsertAsync(User user);
        Task<User> GetByNameAsync(string name);
    }
}