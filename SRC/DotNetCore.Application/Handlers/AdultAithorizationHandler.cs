using DotNetCore.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Application.Handlers
{
    public class AdultAithorizationHandler : AuthorizationHandler<AdultRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdultRequirment requirement)
        {
            var dbo = DateTime.Parse(context.User.FindFirst("Date-Of-Birth")?.Value);
            if (DateTime.Today.Year - dbo.Year >= 18)
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
