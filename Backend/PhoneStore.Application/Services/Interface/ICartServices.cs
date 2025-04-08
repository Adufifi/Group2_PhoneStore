namespace PhoneStore.Application.Services
{
    public interface ICartServices : IBaseServices<Cart>
    {
        Task<bool> DeleteAllAsync(Guid accountIdGuid);
        Task<IEnumerable<Cart>> GetAllCart(Guid accountId);
    }
}
