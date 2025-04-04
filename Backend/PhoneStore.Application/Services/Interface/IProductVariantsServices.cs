namespace PhoneStore.Application.Services.Interface
{
    public interface IProductVariantsServices : IBaseServices<ProductVariants>
    {
        Task<IEnumerable<ProductVariants>> GetAllAsync();

    }
}
