namespace PhoneStore.Application.Services
{
    public class BaseServices<T> : IBaseServices<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddAsync(T entity)
        {
            if (entity != null)
            {
                _unitOfWork.GenericRepository<T>().AddAsync(entity);
                return await _unitOfWork.SaveChangeAsync();
            }
            throw new ArgumentNullException();
        }

        public bool Delete(Guid id)
        {
            _unitOfWork.GenericRepository<T>().Delete(id);
            return _unitOfWork.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _unitOfWork.GenericRepository<T>().Delete(id);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _unitOfWork.GenericRepository<T>().Delete(entity);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.GenericRepository<T>().GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.GenericRepository<T>().GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _unitOfWork.GenericRepository<T>().Update(entity);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}