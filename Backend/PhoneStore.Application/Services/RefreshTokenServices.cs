using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PhoneStore.Application.Services
{
    public class RefreshTokenServices : BaseServices<RefreshToken>, IRefreshTokenServices
    {
        private readonly IConfiguration _configuration;

        public RefreshTokenServices(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
        {
            _configuration = configuration;
        }

        public Task<AuthResultVm> GenerateJwtToken(Account account)
        {
            var authClaim = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, account.Id.ToString()),
                new Claim(ClaimTypes.Email,account.Email)
            };
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
            return null;
        }
    }
}
