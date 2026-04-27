using System;
using System.Collections.Generic;
using System.Text;

namespace Dtos.Auth
{
    public class LoginDataDto
    {
        public required string Login {  get; set; }

        public required string Password { get; set; }
    }
}
