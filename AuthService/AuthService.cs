using AuthService;
using Domain.Exceptions.Account;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Domain.Models.Auth;
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
        private readonly JwtToken _jwtTokenConfiguration;

        public AuthService(IAccountRepsitory accountRepsitory)
        {
            _accountRepsitory = accountRepsitory;
        }

        public async Task<AuthData> Login(string login, string password)
        {
            var passordHash = GeneratePasswordHash(password);

            var acounts = await _accountRepsitory.GetAccountsByLogin(login);

            if (acounts.Count() == 0)
            {
                throw new AccountNotFountException(login);
            }

            Account? account = null;

            foreach (var ac in acounts)
            {
                if (ac.PasswordHash == passordHash)
                {
                    account = ac;
                    break;
                }
            }
            if (account == null) { throw new InvalidAccountDataException(login); }

            return await GenerateTokenPairs(account);
        }

        private async Task<AuthData> GenerateTokenPairs(Account account)
        {
            var accessToken = GenerateAccessToken(account);
            var refreshToken = GenerateRefreshToken();
            var expiresIn = DateTime.UtcNow.Add(_jwtTokenConfiguration.ExpiresIn);

            await _accountRepsitory.UpdateTokenAsync(account.Id, refreshToken, expiresIn);

            return new AuthData(accessToken,
                refreshToken,
                account.Role,
                _jwtTokenConfiguration.ExpiresIn);
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

            var jwt = new JwtSecurityToken(_jwtTokenConfiguration.Issuer,
                _jwtTokenConfiguration.Audience,
                claims,
                expires: DateTime.UtcNow.Add(_jwtTokenConfiguration.ExpiresIn),
                signingCredentials: new SigningCredentials(issuerSigningKey,
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
