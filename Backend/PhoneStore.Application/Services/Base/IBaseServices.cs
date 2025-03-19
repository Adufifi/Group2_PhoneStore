namespace PhoneStore.Application.Services
{
    public interface IBaseServices<T> where T : class
    {
        Task<int> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        bool Delete(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}