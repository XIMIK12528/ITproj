using Domain.Models.Enums;

namespace Domain.Models
{
    public class Account
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Guid Id { get; }
        public bool IsOfferAccepted { get; }
        public string? RefreshToken { get; }
        public AccountRole Role { get; }
        public Account(string login, string passwordHash, Guid id, bool isOfferAccepted, string? refreshToken, AccountRole role)
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
