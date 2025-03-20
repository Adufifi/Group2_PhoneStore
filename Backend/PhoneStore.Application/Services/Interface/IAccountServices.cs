namespace PhoneStore.Application.Services
{
    public interface IAccountServices : IBaseServices<Account>
    {
        Task<Account?> GetAccountByEmail(string email);
        Task<bool> CheckPassAccount(Account account);
    }
}
