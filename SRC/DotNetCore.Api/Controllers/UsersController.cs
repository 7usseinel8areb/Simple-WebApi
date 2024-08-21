using DotNetCore.Infrastructure.Options;
using DotNetCore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using DotNetCore.Domain.RepositoriesInterface;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCore_WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(IUsersRepository _usersRepository) : ControllerBase
    {
        [HttpPost]
        [Route("Auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUser(AuthenticationRequest authRequest)
        {
            string? accessToken = await _usersRepository.CreateToken(authRequest);

            if (accessToken == null) 
                return StatusCode(StatusCodes.Status401Unauthorized,"Username or Password is not valid");

            return Ok(accessToken);
        }
        //Go Here and decode the token https://jwt.io/
    }
}
