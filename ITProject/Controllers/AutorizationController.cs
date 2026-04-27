using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ITProject.Dtos.Auth;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;

namespace ITProject.WebApi.Controllers;

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

    //    [AllowAnonymous]
    //    [HttpPost("refresh")]
    //    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto refreshToken)
    //    {
    //        var accountId = HttpContext.GetAccountId();

    //        var authData = await _accountsService.RefreshAccessTokenAsync(accountId, refreshToken.Token);

    //        return Ok(authData.ToDto());
    //    }

    //    [Authorize(Policy = "OfferIsAccepted")]
    //    [HttpPost("logout")]
    //    public async Task<IActionResult> Logout()
    //    {
    //        var accountId = HttpContext.GetAccountId();

    //        await _accountsService.LogoutAsync(accountId);

    //        return Ok();
    //    }

    //    [HttpPost("offer")]
    //    public async Task<IActionResult> AcceptOffer()
    //    {
    //        var accountId = HttpContext.GetAccountId();

    //        var authData = await _accountsService.AcceptOfferAsync(accountId);

    //        return Ok(authData.ToDto());
    //    }
}
