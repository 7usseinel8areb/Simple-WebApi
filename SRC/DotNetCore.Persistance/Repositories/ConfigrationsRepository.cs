using DotNetCore.Infrastructure.Options;
using DotNetCore.Domain.RepositoriesInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DotNetCore.Persistance.Repositories
{
    public class ConfigurationsRepository : IConfigrationsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<AttachmentOptions> _attachmentOptions;

        /*      //1,2 Options
                public ConfigurationsRepository(IConfiguration configuration,AttachmentOptions attachmentOptions)
                {
                    _configuration = configuration;
                    _attachmentOptions = attachmentOptions;
                }*/



        /*//builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));
                public ConfigurationsRepository(IConfiguration configuration,IOptions<AttachmentOptions> attachmentOptions)
                {
                    _configuration = configuration;
                    _attachmentOptions = attachmentOptions;
                }*/

        /*////////IOptions => Scoped
        ////////IOptionsSnapshot => Singleton
        public ConfigurationsRepository(IConfiguration configuration,IOptionsSnapshot<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
        }*/
        
        public ConfigurationsRepository(IConfiguration configuration,IOptionsMonitor<AttachmentOptions> attachmentOptions)
        {
            _configuration = configuration;
            _attachmentOptions = attachmentOptions;
        }

        public object GetConfigurtations()
        {
            var allowedHosts = new
            {
                AllowedHosts = _configuration["AllowedHosts"],
                ConnectionString = _configuration["ConnectionStrings:con"],//Nested objects
                DefaultLogLevel = _configuration["Logging:LogLevel:Default"],
                TestKey = _configuration["Testing"],
                SigningKey = _configuration["SigningKey"],
                //AttachmentOptions = _attachmentOptions.Value // => Not Working with monitor
                AttachmentOptions = _attachmentOptions.CurrentValue
            };
            return allowedHosts;
        }

    }
}
