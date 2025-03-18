namespace PhoneStore.Application.Services
{
    public class CartServices : BaseServices<Cart>, ICartServices
    {
        public CartServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
