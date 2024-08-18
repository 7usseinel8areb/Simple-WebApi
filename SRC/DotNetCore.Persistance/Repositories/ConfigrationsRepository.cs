using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DotNetCore.Persistance.Repositories
{
    public class ConfigurationsRepository : IConfigrationsRepository
    {
        private readonly IConfiguration _configuration;

        public ConfigurationsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public object GetConfigurtations()
        {
            var allowedHosts = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionString = _configuration["ConnectionStrings:con"],//Nested objects
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["Testing"],
                SigningKey = _configuration["SigningKey"]
            };
            return allowedHosts;
        }

    }
}
