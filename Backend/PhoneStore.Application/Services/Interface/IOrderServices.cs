namespace PhoneStore.Application.Services
{
    public interface IOrderServices : IBaseServices<Order>
    {
        Task<IEnumerable<Order>> GetAllOrderByAccountId(Guid accountId);
        Task<IEnumerable<Order>> GetAllOrder();
    }
}



