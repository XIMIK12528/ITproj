using Domain.Interfaces.Repositories;
using Dtos.Auth;
using ITProject.Extensions; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITProject.WebApi.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDataDto loginData)
        {
            var authData = await _authService.Login(loginData.Login, loginData.Password);
            return Ok(authData.ToDto());
        }

        [Authorize] 
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshToken)
        {
            var accountId = HttpContext.GetAccountId();
            var authData = await _authService.RefreshAccessTokenAsync(accountId, refreshToken.Token);

            return Ok(authData.ToDto());
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var accountId = HttpContext.GetAccountId();
            await _authService.LogoutAsync(accountId);

            return Ok();
        }
    }
}