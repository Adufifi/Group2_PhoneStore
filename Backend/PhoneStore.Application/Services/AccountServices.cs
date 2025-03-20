namespace PhoneStore.Application.Services
{
    public class AccountServices : BaseServices<Account>, IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckPassAccount(Account account)
        {
            var checkAccount = await _unitOfWork.GenericRepository<Account>().GetQuery(c => c.Email == account.Email);
            var existingAccount = await checkAccount.FirstOrDefaultAsync();
            if (existingAccount == null)
                return false;
            return VerifyPassword(account.PassWord, existingAccount.PassWord);
        }
        public async Task<Account?> GetAccountByEmail(string email)
        {
            var account = await _unitOfWork.GenericRepository<Account>().GetQuery(c => c.Email == email);
            return await account.FirstOrDefaultAsync();
        }
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
