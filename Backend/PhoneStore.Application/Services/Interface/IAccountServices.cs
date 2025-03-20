using PhoneStore.Domain.ModelRequest;

namespace PhoneStore.Application.Services
{
    public interface IAccountServices : IBaseServices<Account>
    {
        Task<Account?> GetAccountByEmail(string email);
        bool CheckPassAccount(Account account, LoginVm login);
    }
}
