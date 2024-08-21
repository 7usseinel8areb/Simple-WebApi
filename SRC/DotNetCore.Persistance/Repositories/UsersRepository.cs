using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.Infrastructure.Options;
using DotNetCore.Domain.Models;
using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using DotNetCore.Domain.Enums;
namespace DotNetCore.Persistance.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        private readonly JwtOptions _jwtOptions;

        public UsersRepository(JwtOptions jwtOptions, string connectionString)
            : base(connectionString)
        {
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> CheckUserPermission(int userId,Permession permession)
        {
            bool hasPermission = false;
            using(var connection = await GetOpenConnectionAsync())
            {
                using(var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "CheckUserPermission";
                    command.Parameters.Add("UserId", SqlDbType.Int).Value = userId;
                    command.Parameters.Add("PermissionId", SqlDbType.Int).Value = permession;

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            hasPermission = reader.GetInt32(0) == 1;
                        }
                    }
                }
            }
            return hasPermission;
        }

        public async Task<string?> CreateToken(AuthenticationRequest authRequest)
        {
            User? user = await GetUserAsync(authRequest);

            if (user is null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            // User and token information
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Token information
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,

                // Signing credentials with key and algorithm
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                ),

                // User Information => Header
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, "halghryb287@gmail.com"),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Role, SystemUsers.SuperAdmin.ToString())
                    }
                )
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;
        }

        public async Task<User?> GetUserAsync(AuthenticationRequest authRequest)
        {
            User user = new User();
            using(var connection = await GetOpenConnectionAsync())
            {
                using(var command = connection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "GetUserByUserNameAndPassword";
                    command.Parameters.Add("UserName", SqlDbType.NVarChar).Value = authRequest.UserName;
                    command.Parameters.Add("Password", SqlDbType.NVarChar).Value = authRequest.Password;

                    using(var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user.Id = reader.GetInt32(0);
                            user.Name = reader.GetString(1);
                            user.Password = reader.GetString(2);
                        }
                        else
                            return null;
                    }
                }
            }
            return user;
        }
    }
}
