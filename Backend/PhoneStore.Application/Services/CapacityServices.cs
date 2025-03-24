namespace PhoneStore.Application.Services
{
    public class CapacityServices : BaseServices<Capacity>, ICapacityServices
    {
        public CapacityServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
