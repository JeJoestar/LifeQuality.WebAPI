using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LifeQuality.WebAPI.Controllers
{
    [ApiController]
    [Route("authentication")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly ILoginService _loginService;

        public AuthenticationController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogInRequest userLogInDto)
        {
            var authenticateResponse = await _loginService.AuthenticateAsync(userLogInDto.Email, userLogInDto.Password, false);

            if (authenticateResponse == null)
            {
                return Unauthorized("NameOrPasswordIncorrect");
            }

            return Ok(authenticateResponse);
        }
    }
}
