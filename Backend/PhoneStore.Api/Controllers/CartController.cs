namespace PhoneStore.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;
        private readonly IMapper _mapper;

        public CartController(ICartServices cartServices, IMapper mapper)
        {
            _cartServices = cartServices;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public IActionResult GetCartByUserId(Guid userId)
        {
            var carts = _cartServices.GetCartByUserId(userId);
            if (carts == null)
                return NotFound();
            return Ok(carts);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _cartServices.AddProductToCart(cart);
            if (!result)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{productId}/{userId}")]
        public IActionResult DeleteFromCart(Guid productId, Guid userId)
        {
            var result = _cartServices.DeleteProductFromCart(productId, userId);
            if (!result)
                return NotFound();
            return Ok();
        }
    }
}
