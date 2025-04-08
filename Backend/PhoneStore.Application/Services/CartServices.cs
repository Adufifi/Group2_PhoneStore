
namespace PhoneStore.Application.Services
{
    public class CartServices : BaseServices<Cart>, ICartServices
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public CartServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWOrk = unitOfWork;
        }

        public Task<bool> DeleteAllAsync(Guid accountIdGuid)
        {

            var carts = _unitOfWOrk.GenericRepository<Cart>()
                .Get(filter: c => c.AccountId == accountIdGuid)
                .ToList();
            if (carts.Count > 0)
            {
                foreach (var cart in carts)
                {
                    _unitOfWOrk.GenericRepository<Cart>().Delete(cart);
                }
                return Task.FromResult(_unitOfWOrk.SaveChangeAsync().Result > 0);
            }
            return Task.FromResult(false);
        }

        public async Task<IEnumerable<Cart>> GetAllCart(Guid accountId)
        {
            return await Task.FromResult(
                    _unitOfWOrk.GenericRepository<Cart>()
                    .Get(filter: c => c.AccountId == accountId
                        , includeProperties: "ProductVariants,ProductVariants.Product,ProductVariants.ProductColor,ProductVariants.Capacity")
                    .ToList()
                );
        }
    }
}
