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

        [HttpGet("GetAllCart")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
        {
            var carts = await _cartServices.GetAllAsync();
            return Ok(carts);
        }
        [HttpGet("GetCartByAccountId/{accountId}")]
        public async Task<IActionResult> GetCartByAccountId(string accountId)
        {
            if (!Guid.TryParse(accountId, out Guid accountIdGuid))
            {
                return BadRequest("Invalid account ID format");
            }
            var carts = await _cartServices.GetAllCart(accountIdGuid);

            var cartByAccountId = carts.Where(c => c.AccountId == accountIdGuid).ToList();
            var cartDtoList = cartByAccountId.Select(c => new CartDto
            {
                Id = c.Id,
                ProductName = c.ProductVariants.Product.ProductName,
                ColorName = c.ProductVariants.ProductColor.ColorName,
                CapacityName = c.ProductVariants.Capacity.CapacityName,
                Price = c.ProductVariants.Price,
                Quantity = c.Quantity,
                Image = c.ProductVariants.Product.Image
            });
            return Ok(cartDtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetById(Guid id)
        {
            var cart = await _cartServices.GetByIdAsync(id);
            if (cart == null)
                return NotFound($"Cart with id {id} not found");
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        public async Task<IActionResult> Create([FromBody] AddCart cart)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return Ok(statusResponse);
            }
            if (!Guid.TryParse(cart.ProductVariantId, out Guid productVariantsId))
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return Ok(statusResponse);
            }
            if (!Guid.TryParse(cart.AccountId, out Guid accountId))
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return Ok(statusResponse);
            }
            var cartEntity = new Cart
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                ProductVariantsId = productVariantsId,
                CreatedDate = DateTime.Now,
                Quantity = 1
            };
            var result = await _cartServices.AddAsync(cartEntity);
            if (result > 0)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Add success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Failed to add cart";
            return Ok(statusResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != cart.Id)
                return BadRequest("Id mismatch");

            var existingCart = await _cartServices.GetByIdAsync(id);
            if (existingCart == null)
                return NotFound($"Cart with id {id} not found");

            cart.CreatedDate = existingCart.CreatedDate;
            var result = await _cartServices.UpdateAsync(cart);
            if (!result)
                return BadRequest("Failed to update cart");

            return NoContent();
        }
        [HttpDelete("DeleteAllCartByAccountId/{accountId}")]
        public async Task<IActionResult> DeleteAll(string accountId)
        {
            var statusResponse = new StatusResponse();
            if (!Guid.TryParse(accountId, out Guid accountIdGuid))
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return Ok(statusResponse);
            }
            var result = await _cartServices.DeleteAllAsync(accountIdGuid);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Failed to delete cart";
            return Ok(statusResponse);
        }


        [HttpDelete("DeleteCartById/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var statusResponse = new StatusResponse();
            if (!Guid.TryParse(id, out Guid cartId))
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return Ok(statusResponse);
            }
            var result = await _cartServices.DeleteAsync(cartId);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Failed to delete cart";
            return Ok(statusResponse);

        }

    }
}
