using DbModels;

public class RefreshTokenDb
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountDb? Account { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }

    public RefreshTokenDb(Guid id, Guid accountId, string refreshToken, DateTimeOffset expiresIn)
    {
        Id = id;
        AccountId = accountId;
        RefreshToken = refreshToken;
        ExpiresIn = expiresIn;
    }
}