
namespace PhoneStore.Application.Services
{
    public class ProductVariantsServices : BaseServices<ProductVariants>, IProductVariantsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductVariantsServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductVariants>> GetAllAsync()
        {
            return await Task.FromResult(
             _unitOfWork.GenericRepository<ProductVariants>()
             .Get(includeProperties: "Product,ProductColor,Capacity")
             .ToList());
        }
    }
}
