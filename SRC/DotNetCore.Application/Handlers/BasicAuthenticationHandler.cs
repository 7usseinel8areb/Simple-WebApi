using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DotNetCore.Application.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> optionsMonitor, ILoggerFactory logger, UrlEncoder urlEncoder, ISystemClock systemClock) 
            : base(optionsMonitor, logger, urlEncoder, systemClock)
        {
            
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //We have very important header name authorization
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            if(!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"],out var authHeader))
                return Task.FromResult(AuthenticateResult.Fail("Unkown scheme"));

            if(!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Unkown scheme"));

            var encodedCredentials = authHeader.Parameter;
            string decodedCredentials;
            try
            {
                decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

            }catch(Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format."));
            }

            var userNameAndPassword = decodedCredentials.Split(':');

            if (userNameAndPassword[0] != "admin" || userNameAndPassword[1] != "password")
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));

            //Returning Success Requires ClaimsIdentity, ClaimsPrincipal

            //Claims => Means User information
            ClaimsIdentity identity = new(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,"1"),//No repetation Identity
                new Claim(ClaimTypes.Name, userNameAndPassword[0]),

                //Don't put the password at a claim
            }, "Basic");// Claims, Auth type

            ClaimsPrincipal principal = new(identity);//one prinncipal can take one or more identity

            AuthenticationTicket ticket = new(principal, "Basic");
            
            //تذكرة الدخول
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
