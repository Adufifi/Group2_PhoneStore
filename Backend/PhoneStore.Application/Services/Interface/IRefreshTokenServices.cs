using PhoneStore.Domain.ViewModel;

namespace PhoneStore.Application.Services.Interface
{
    public interface IRefreshTokenServices : IBaseServices<RefreshToken>
    {
        Task<AuthResultVm> GenerateJwtToken(Account account);
    }
}
