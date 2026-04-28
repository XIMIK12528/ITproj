using Domain.Models;
using DbModels.Converters.Enums;

namespace DbModels.Converters
{
    public static class AccountDbConverter
    {
        public static AccountDb ToDb(this Account account)
        {
            return new AccountDb
            (
                id: account.Id,
                login: account.Login,
                passwordHash: account.PasswordHash,
                isOfferAccepted: account.IsOfferAccepted,
                refreshToken: account.RefreshToken,
                role: account.Role.ToDb()
            );
        }
        public static Account ToDomain(this AccountDb accountDb)
        {
            return new Account
            (
                id: accountDb.Id,
                login: accountDb.Login,
                passwordHash: accountDb.PasswordHash,
                isOfferAccepted: accountDb.IsOfferAccepted,
                refreshToken: accountDb.RefreshToken,
                role: accountDb.Role.ToDomain()
            );
        }
    }
}
