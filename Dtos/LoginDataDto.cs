using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Auth
{
    public class LoginDataDto
    {
        public required string login {  get; set; }

        public required string Password { get; set; }
    }
}
