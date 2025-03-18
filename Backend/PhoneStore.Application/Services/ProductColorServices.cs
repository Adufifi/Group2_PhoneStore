
namespace PhoneStore.Application.Services
{
    public class ProductColorServices : BaseServices<ProductColor>, IProductColorServices
    {
        public ProductColorServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
