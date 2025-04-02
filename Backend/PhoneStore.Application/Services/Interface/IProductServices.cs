﻿namespace PhoneStore.Application.Services.Interface
{
    public interface IProductServices : IBaseServices<Product>
    {
        Task<IEnumerable<Product>> GetAlllAsync();
        //Task<Product?> GetByIddAsync(Guid id);
    }
}
