using AuthService;
using Domain.Exceptions.Account;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepsitory _accountRepsitory;
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        public AuthService(IAccountRepsitory accountRepsitory, IOptions<JwtTokenConfiguration> jwtTokenConfiguration)
        {
            _accountRepsitory = accountRepsitory;
            _jwtTokenConfiguration = jwtTokenConfiguration.Value;
        }

        public async Task<AuthData> Login(string login, string password)
        {
            var passordHash = GeneratePasswordHash(password);
            var acounts = await _accountRepsitory.GetAccountsByLogin(login);

            if (!acounts.Any())
                throw new AccountNotFountException(login);

            var account = acounts.FirstOrDefault(ac => ac.PasswordHash == passordHash);

            if (account == null)
                throw new InvalidAccountDataException(login);

            return await GenerateTokenPairs(account);
        }

        public async Task<AuthData> RefreshAccessTokenAsync(Guid accountId, string refreshToken)
        {
            var dbRefreshToken = await _accountRepsitory.GetRefreshTokenAsync(accountId);

            if (string.IsNullOrEmpty(dbRefreshToken) || dbRefreshToken != refreshToken)
            {
                throw new Exception("Invalid refresh token"); // Замени на свой кастомный Exception, например InvalidException
            }

            var account = await _accountRepsitory.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                throw new AccountNotFountException(accountId.ToString());
            }

            return await GenerateTokenPairs(account);
        }

        // НОВОЕ: Логика выхода (удаление рефреш токена из БД)
        public async Task LogoutAsync(Guid accountId)
        {
            await _accountRepsitory.RemoveTokenAsync(accountId);
        }

        private async Task<AuthData> GenerateTokenPairs(Account account)
        {
            var accessToken = GenerateAccessToken(account);
            var refreshToken = GenerateRefreshToken();
            var expiresIn = DateTime.UtcNow.Add(_jwtTokenConfiguration.ExpiresIn);

            await _accountRepsitory.UpdateTokenAsync(account.Id, refreshToken, expiresIn);

            return new AuthData(accessToken, refreshToken, account.Role, _jwtTokenConfiguration.ExpiresIn);
        }

        private static string GeneratePasswordHash(string password)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private string GenerateAccessToken(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(AuthClaims.AccountId, account.Id.ToString()),
            };

            var keyBytes = Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key);
            var issuerSigningKey = new SymmetricSecurityKey(keyBytes);

            var jwt = new JwtSecurityToken(
                _jwtTokenConfiguration.Issuer,
                _jwtTokenConfiguration.Audience,
                claims,
                expires: DateTime.UtcNow.Add(_jwtTokenConfiguration.ExpiresIn),
                signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}