using System;
using System.Security.Claims;
using Domain.Models.Auth;
using Microsoft.AspNetCore.Http;

namespace ITProject.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetAccountId(this HttpContext context)
        {
            var claim = context.User.FindFirst(AuthClaims.AccountId);
            if (claim != null && Guid.TryParse(claim.Value, out Guid id))
            {
                return id;
            }
            throw new UnauthorizedAccessException("User is not authorized.");
        }
    }
}