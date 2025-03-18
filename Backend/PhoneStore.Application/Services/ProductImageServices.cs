
namespace PhoneStore.Application.Services
{
    public class ProductImageServices : BaseServices<ProductImage>, IProductImageServices
    {
        public ProductImageServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
