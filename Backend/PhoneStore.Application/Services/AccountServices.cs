using PhoneStore.Domain.ModelRequest;

namespace PhoneStore.Application.Services
{
    public class AccountServices : BaseServices<Account>, IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckPassAccount(Account account, LoginVm login)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(login.Password, account.PassWord);
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            var account = await _unitOfWork.GenericRepository<Account>().GetQuery(c => c.Email == email);
            return await account.FirstOrDefaultAsync();
        }


    }
}
