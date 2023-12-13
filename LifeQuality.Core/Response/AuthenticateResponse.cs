using LifeQuality.DataContext.Model;

namespace LifeQuality.Core.Response
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JwtToken { get; set; }
        public UserType Role { get; set; }
        public AuthenticateResponse() { }
        public AuthenticateResponse(User user, string jwtToken)
        {
            Id = user.Id;
            Name = user.Name;
            JwtToken = jwtToken;
            Role = user.UserType;
        }
    }
}