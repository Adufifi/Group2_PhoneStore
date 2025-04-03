using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PhoneStore.Authentication
{
    public class XClientSourceAuthenticationHandlerOption : AuthenticationSchemeOptions
    {
        public string IssuerSigningKey { get; set; } = string.Empty;
        public Func<string, JwtSecurityToken, ClaimsPrincipal, Task<bool>> ClientValidator { get; set; } = (token, jwtToken, pricipal) => Task.FromResult(true);
    }
}
