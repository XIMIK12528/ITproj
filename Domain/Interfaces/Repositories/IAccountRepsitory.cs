using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IAccountRepsitory
    {
        Task<List<Account>> GetAccountsByLogin(string login);
        Task<Account?> GetAccountByIdAsync(Guid accountId);
        Task UpdateTokenAsync(Guid accountId, string refreshToken, DateTimeOffset expiresIn);
        Task<string?> GetRefreshTokenAsync(Guid accountId); 
        Task RemoveTokenAsync(Guid accountId); 
    }
}
