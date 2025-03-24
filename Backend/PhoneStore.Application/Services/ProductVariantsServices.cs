
namespace PhoneStore.Application.Services
{
    public class ProductVariantsServices : BaseServices<ProductVariants>, IProductVariantsServices
    {
        public ProductVariantsServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
