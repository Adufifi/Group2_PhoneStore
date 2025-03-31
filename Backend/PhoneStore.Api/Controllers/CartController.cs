namespace PhoneStore.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetAll()
        {
            var carts = await _cartServices.GetAllAsync();
            return Ok(carts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetById(Guid id)
        {
            var cart = await _cartServices.GetByIdAsync(id);
            if (cart == null)
                return NotFound($"Cart with id {id} not found");
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> Create([FromBody] Cart cart)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            cart.CreatedDate = DateTime.UtcNow;
            var result = await _cartServices.AddAsync(cart);
            if (result <= 0)
                return BadRequest("Failed to create cart");

            return CreatedAtAction(nameof(GetById), new { id = cart.Id }, cart);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingCart = await _cartServices.GetByIdAsync(id);
            if (existingCart == null)
                return NotFound($"Cart with id {id} not found");

            var result = await _cartServices.DeleteAsync(id);
            if (!result)
                return BadRequest("Failed to delete cart");

            return NoContent();
        }

    }
}
