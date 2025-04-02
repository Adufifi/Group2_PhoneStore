


namespace PhoneStore.Application.Services
{
    public class ProductServices : BaseServices<Product>, IProductServices
    {
        private readonly IUnitOfWork _unitOfWod;

        public ProductServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWod = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAlllAsync()
        {
            return await Task.FromResult(
             _unitOfWod.GenericRepository<Product>()
             .Get(includeProperties: "Brand")
             .ToList());
        }

        //public Task<Product?> GetByIddAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
