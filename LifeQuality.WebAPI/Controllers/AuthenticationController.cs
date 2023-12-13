using LifeQuality.Core.DTOs.Users;
using LifeQuality.Core.Response;
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
        [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserLogInRequest userLogInDto)
        {
            var authenticateResponse = await _loginService.AuthenticateAsync(userLogInDto.Email, userLogInDto.Password, false);

            if (authenticateResponse == null)
            {
                return BadRequest("Email or Password is incorrect");
            }

            return Ok(authenticateResponse);
        }
    }
}
