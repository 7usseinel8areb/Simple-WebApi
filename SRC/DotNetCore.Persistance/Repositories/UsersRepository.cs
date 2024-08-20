using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.Application.Options;
using DotNetCore.Domain.Models;
using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DotNetCore.Persistance.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly JwtOptions _jwtOptions;

        // Constructor with Dependency Injection
        public UsersRepository(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public Task<string> CreateToken(AuthenticationRequest authRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // User and token information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token information

                // Options => PayLoad info
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,

                // Need Key and Algorithm
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                ),

                // User Information => Header
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, authRequest.UserName),
                        new Claim(ClaimTypes.Email, "halghryb287@gmail.com"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }
                )
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Task.FromResult(accessToken);
        }
    }
}
