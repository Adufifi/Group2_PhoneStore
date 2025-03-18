
namespace PhoneStore.Application.Services
{
    public class OrderItemServices : BaseServices<OrderItem>, IOrderItemServices
    {
        public OrderItemServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
