using Domain.Models;
using DbModels.Enums;
using DataAccsess.Context;
using Microsoft.EntityFrameworkCore;
using DbModels.Converters;
using Domain.Interfaces.Repositories;

namespace Repositories
{
    public class AccountRepsitory : IAccountRepsitory
    {
        private readonly Context _context;  

        public AccountRepsitory(Context context)
        {
            _context = context;
        }

        public async Task<List<Account>> GetAccountsByLogin(string login)
        {
            var accounts = await _context.Accounts.AsNoTracking()
                .Where(a => a.Role != AccountRoleDb.guest)
                .Where(a => a.Login == login)
                .ToListAsync();

            return accounts.ConvertAll(a => a.ToDomain());
        }

        public async Task UpdateTokenAsync(Guid accountId, string refreshToken, DateTimeOffset expiresIn)
        {
            var token = await _context.RefreshTokens.SingleOrDefaultAsync(t => t.AccountId == accountId);

            if (token == null)
            {
                token = new RefreshTokenDb(id: Guid.NewGuid(),
                    accountId: accountId,
                    refreshToken: refreshToken,
                    expiresIn: expiresIn);
                await _context.RefreshTokens.AddAsync(entity: token);
            }
            else
            {
                token.RefreshToken = refreshToken;
                token.ExpiresIn = expiresIn;
            }
        }
    }
}
