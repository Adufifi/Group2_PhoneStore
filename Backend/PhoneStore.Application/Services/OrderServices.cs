
namespace PhoneStore.Application.Services
{
    public class OrderServices : BaseServices<Order>, IOrderServices
    {
        public OrderServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
