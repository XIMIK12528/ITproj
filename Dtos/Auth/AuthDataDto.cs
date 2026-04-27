using Domain.Models.Auth;

namespace Dtos.Auth
{
    public class AuthDataDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public string Type { get; set; }
        public double ExpiresInSeconds { get; set; }
    }
    public static class AuthConverter
    {
        public static AuthDataDto ToDto(this AuthData data)
        {
            return new AuthDataDto
            {
                AccessToken = data.AccessToken,
                RefreshToken = data.RefreshToken,
                Type = data.Type.ToString(),
                ExpiresInSeconds = data.ExpiresIn.TotalSeconds
            };
        }
    }
}

