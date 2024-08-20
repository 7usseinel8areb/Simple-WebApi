using DotNetCore.Application.Options;
using DotNetCore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using DotNetCore.Domain.RepositoriesInterface;
using System.Text.Json;

namespace DotNetCore_WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController(IUsersRepository _usersRepository) : ControllerBase
    {
        [HttpPost]
        [Route("Auth")]
        public async Task<IActionResult> AuthenticateUser(AuthenticationRequest authRequest)
        {
            var accessToken = await _usersRepository.CreateToken(authRequest);

            return Ok(accessToken);
        }
        //Go Here and decode the token https://jwt.io/
    }
}
