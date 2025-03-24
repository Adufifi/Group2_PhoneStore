namespace PhoneStore.Application.Services
{
    public class BrandServices : BaseServices<Brand>, IBrandServices
    {
        public BrandServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
