using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;

namespace PhoneStore.Authentication
{
    public class XCLientSourceAuthenticationHandler : AuthenticationHandler<XClientSourceAuthenticationHandlerOption>
    {
        public XCLientSourceAuthenticationHandler(IOptionsMonitor<XClientSourceAuthenticationHandlerOption> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("token", out var tokenHeader) || string.IsNullOrWhiteSpace(tokenHeader))
            {
                Logger.LogWarning("Missing token in request headers.");
                return AuthenticateResult.Fail("Missing token");
            }
            var tokenValue = tokenHeader.FirstOrDefault();
            var principal = ValidateToken(tokenValue, out var jwtToken);
            if (principal is null || jwtToken is null)
            {
                Logger.LogWarning("Invalid JWT token.");
                return AuthenticateResult.Fail("Invalid token");
            }
            if (!await Options.ClientValidator(tokenValue, jwtToken, principal))
            {
                Logger.LogWarning("Client validation failed.");
                return AuthenticateResult.Fail("Client validation failed");
            }
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);


        }

        private object ValidateToken(string? tokenValue, out JwtSecurityToken jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}
