using Domain.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITProject.Middlewears
{
    public class OfferIsAcceptedRequirement : IAuthorizationRequirement
    {
    }
    public class OfferIsAcceptedMiddleware : AuthorizationHandler<OfferIsAcceptedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OfferIsAcceptedRequirement requirement)
        {
            var isOfferAcceptedClaim = context.User.Claims
                .FirstOrDefault(c => c.Type == AuthClaims.IsOfferAccepted);

            if (isOfferAcceptedClaim == null)
            {
                context.Fail();
            }
            else
            {
                if (bool.Parse(isOfferAcceptedClaim.Value))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
