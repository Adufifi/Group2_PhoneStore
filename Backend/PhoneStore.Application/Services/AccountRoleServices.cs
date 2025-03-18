namespace PhoneStore.Application.Services
{
    public class AccountRoleServices : BaseServices<AccountRole>, IAccountRoleServices
    {
        public AccountRoleServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
