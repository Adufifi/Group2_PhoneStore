


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

        public async Task<IEnumerable<Product>> GetNewProductByBrand(IEnumerable<Brand> list)
        {
            var brandIds = list.Select(b => b.Id).ToList();
            var products = await _unitOfWod.GenericRepository<Product>()
                .Get(filter: p => brandIds.Contains(p.BrandId), includeProperties: "Brand")
                .ToListAsync();
            return products;
        }
    }
}
