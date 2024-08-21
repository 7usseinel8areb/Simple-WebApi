using DotNetCore.Application.Validators;
using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DotNetCore_WebApi.Filters
{
    public class PermessionBasedAuthorizationFilter(IUsersRepository _usersRepository) : IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var attribute =(CheckPermissionAttribute?)context.ActionDescriptor.EndpointMetadata
                .FirstOrDefault(att => att is CheckPermissionAttribute);

            if(attribute != null)
            {
                ClaimsIdentity? claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimsIdentity is null || !claimsIdentity.IsAuthenticated)
                    context.Result = new ForbidResult();
                else
                {
                    var userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                    bool hasPermission =await _usersRepository.CheckUserPermission(userId,attribute.Permession);

                    if (!hasPermission)
                        context.Result = new ForbidResult();

                }

            }
        }
    }
}
