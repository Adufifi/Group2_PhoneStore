using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PhoneStore.Authentication
{
    public class XClientSourceAuthenticationHandler : AuthenticationHandler<XClientSourceAuthenticationHandlerOption>
    {
        public XClientSourceAuthenticationHandler(IOptionsMonitor<XClientSourceAuthenticationHandlerOption> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var tokenHeader = Request.Headers["Authorization"];
            if (tokenHeader.Count == 0)
            {
                Logger.LogWarning("No token found in the request header.");
                return AuthenticateResult.NoResult();
            }
            if (tokenHeader.Count > 1)
            {
                Logger.LogWarning("Multiple tokens found in the request header.");
                return AuthenticateResult.NoResult();
            }
            var tokenValue = tokenHeader.FirstOrDefault()?.Replace("Bearer ", string.Empty);
            var principal = ValidateToken(tokenValue, out JwtSecurityToken jwtToken);

            if (principal is null || jwtToken is null)
            {
                Logger.LogWarning("Invalid JWT token.");
                return AuthenticateResult.Fail("Invalid token");
            }
            if (!await Options.ClientValidator(tokenValue!, jwtToken, principal))
            {
                Logger.LogWarning("Client validation failed.");
                return AuthenticateResult.Fail("Client validation failed");
            }
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
        private ClaimsPrincipal ValidateToken(string? tokenValue, out JwtSecurityToken jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Convert.FromBase64String(Options.IssuerSigningKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = handler.ValidateToken(tokenValue, parameters, out SecurityToken securityToken);
                jwtToken = securityToken as JwtSecurityToken;
                return principal;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Token validation failed.");
                jwtToken = null!;
                return null!;
            }
        }
    }
}
