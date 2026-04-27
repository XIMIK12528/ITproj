using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IAccountRepsitory
    {
        Task<List<Account>> GetAccountsByLogin(string login);

        Task UpdateTokenAsync(Guid accountId, string refreshToken, DateTimeOffset expiresIn);
    }
}
