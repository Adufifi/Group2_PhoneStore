namespace PhoneStore.Application.Services
{
    public class AccountServices : BaseServices<Account>, IAccountServices
    {
        public AccountServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
