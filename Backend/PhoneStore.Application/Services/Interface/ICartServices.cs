namespace PhoneStore.Application.Services
{
public interface ICartServices : IBaseServices<Cart>
{
    IEnumerable<Cart> GetCartByUserId(Guid userId); 
    bool AddProductToCart(Cart cart); 
    bool DeleteProductFromCart(Guid productId, Guid userId); 
}
}
