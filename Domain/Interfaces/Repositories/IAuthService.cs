using Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IAuthService
    {
        Task<AuthData> Login(string login, string password);
        Task<AuthData> RefreshAccessTokenAsync(Guid accountId, string refreshToken);
        Task LogoutAsync(Guid accountId);
    }
}
