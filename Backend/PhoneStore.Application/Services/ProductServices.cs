﻿

namespace PhoneStore.Application.Services
{
    public class ProductServices : BaseServices<Product>, IProductServices
    {
        public ProductServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
