using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Auth
{
    public class AuthData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public AccountRole Type { get; set; }
        public TimeSpan ExpiresIn { get; set; }

        public AuthData(string accessToken, string refreshToken, AccountRole type, TimeSpan expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Type = type;
            ExpiresIn = expiresIn;
        }
    }
}