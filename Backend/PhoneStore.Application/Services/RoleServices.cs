﻿
namespace PhoneStore.Application.Services
{
    public class RoleServices : BaseServices<Role>, IRoleServices
    {
        public RoleServices(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
