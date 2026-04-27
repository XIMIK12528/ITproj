using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService
{
    public class JwtTokenConfiguration
    {
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required TimeSpan ExpiresIn { get; set; }
        public required string Key { get; set; }
    }
}
