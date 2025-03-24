using PhoneStore.Infrastructure.DataContext;
using PhoneStore.Domain.Models;

namespace PhoneStore.Application.Services
{
    public class CartServices : BaseServices<Cart>, ICartServices
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CartServices(AppDbContext context, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Cart> GetCartByUserId(Guid userId)
        {
            return _context.Carts.Where(c => c.AccountId == userId).ToList();
        }

        public bool AddProductToCart(Cart cart)
        {
            _context.Carts.Add(cart);
            return _unitOfWork.SaveChanges() > 0;
        }

        

        public bool DeleteProductFromCart(Guid productId, Guid userId)
        {
            var cartItem = _context.Carts.FirstOrDefault(c => c.ProductId == productId && c.AccountId == userId);
            if (cartItem == null)
            {
                return false;
            }

            _context.Carts.Remove(cartItem);
            return _unitOfWork.SaveChanges() > 0;
        }
    }
}
