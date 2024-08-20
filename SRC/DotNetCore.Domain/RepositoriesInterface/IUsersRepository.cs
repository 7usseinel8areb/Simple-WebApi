using DotNetCore.Domain.Enums;
using DotNetCore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Domain.RepositoriesInterface
{
    public interface IUsersRepository
    {
        Task<string?> CreateToken(AuthenticationRequest authRequest);

        Task<bool> CheckUserPermission(int userId, Permession permession);
    }
}
