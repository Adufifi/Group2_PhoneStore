

namespace PhoneStore.Application.Services
{
    public class OrderServices : BaseServices<Order>, IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Order>> GetAllOrderByAccountId(Guid accountId)
        {
            var order = _unitOfWork.GenericRepository<Order>()
            .Get(filter: o => o.AccountId == accountId,
                includeProperties: "ProductVariants,ProductVariants.Product,ProductVariants.ProductColor,ProductVariants.Capacity").ToList();
            return Task.FromResult<IEnumerable<Order>>(order);
        }
        public Task<IEnumerable<Order>> GetAllOrder()
        {
            var order = _unitOfWork.GenericRepository<Order>()
            .Get(includeProperties: "ProductVariants,ProductVariants.Product,ProductVariants.ProductColor,ProductVariants.Capacity").ToList();
            return Task.FromResult<IEnumerable<Order>>(order);
        }

    }
}
