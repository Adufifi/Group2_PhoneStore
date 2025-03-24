namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageServices _productImageServices;

        public ProductImageController(IProductImageServices productImageServices)
        {
            _productImageServices = productImageServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductImage>>> GetAll()
        {
            var images = await _productImageServices.GetAllAsync();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductImage>> GetById(Guid id)
        {
            var image = await _productImageServices.GetByIdAsync(id);
            if (image == null)
                return NotFound();
            return Ok(image);
        }

        [HttpPost]
        public async Task<ActionResult<ProductImage>> Create(ProductImage productImage)
        {
            productImage.CreatedDate = DateTime.UtcNow;
            await _productImageServices.AddAsync(productImage);
            return CreatedAtAction(nameof(GetById), new { id = productImage.Id }, productImage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ProductImage productImage)
        {
            if (id != productImage.Id)
                return BadRequest();

            var existingImage = await _productImageServices.GetByIdAsync(id);
            if (existingImage == null)
                return NotFound();

            var result = await _productImageServices.UpdateAsync(productImage);
            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productImageServices.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
