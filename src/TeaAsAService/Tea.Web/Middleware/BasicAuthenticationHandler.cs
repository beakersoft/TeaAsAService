using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tea.Core.Data;
using Tea.Core.Domain;

namespace Tea.Web.Middleware
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static readonly string SchemeName = "BasicAuthentication";
        private readonly IDataStore _dataStore;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IDataStore dataStore)
            : base(options, logger, encoder, clock)
        {
            _dataStore = dataStore;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Logger.LogInformation("Missing Authorization Header", Request.Headers);
                return AuthenticateResult.Fail("Missing Authorization Header");
            }
                
            User user;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                user = await _dataStore.AuthenticateAsync(username, password);
            }
            catch(Exception ex)
            {
                Logger.LogError("Basic authentication failed", Request.Headers["Authorization"],ex);
                return AuthenticateResult.Fail("Basic authentication failed. Unable to parse username and password");
            }

            if (user == null)
            {
                Logger.LogWarning("Invalid Username or Password", Request.Headers["Authorization"]);
                return AuthenticateResult.Fail("Invalid Username or Password");
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (!string.IsNullOrEmpty(user.EmailAddress))
                claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }    
}
