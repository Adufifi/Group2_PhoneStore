
namespace PhoneStore.Application.Services
{
    public class ReviewServices : BaseServices<Review>, IReviewServices
    {
        public ReviewServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
