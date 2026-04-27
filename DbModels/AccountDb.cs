using DbModels.Enums;

namespace DbModels
{
    public class AccountDb
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Guid Id { get; set; }
        public bool IsOfferAccepted { get; set; }
        public string? RefreshToken { get; set; }
        public AccountRoleDb Role { get; set; } 
        public AccountDb(string login, string passwordHash, Guid id, bool isOfferAccepted, string? refreshToken, AccountRoleDb role)
        {
            Login = login;
            PasswordHash = passwordHash;
            Id = id; 
            IsOfferAccepted = isOfferAccepted;
            RefreshToken = refreshToken;
            Role = role;
        }
    }
}
