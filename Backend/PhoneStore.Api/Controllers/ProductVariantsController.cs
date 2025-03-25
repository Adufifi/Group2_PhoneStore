using PhoneStore.Domain.Models;

namespace PhoneStore.Api.Controllers
{
    [Route("api/productvariants")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantsServices _productVariantServices;
        private readonly IMapper _mapper;

        public ProductVariantsController(IProductVariantsServices productVariantServices, IMapper mapper)
        {
            _productVariantServices = productVariantServices;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var data = await _productVariantServices.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("GetVariantById/{id}")]
        public async Task<IActionResult> GetVariantById(Guid id)
        {
            var data = await _productVariantServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("CreateVariant")]
        public async Task<IActionResult> CreateVariant([FromBody] ProductVariantDto productVariantDto)
        {
            var dataAdd = _mapper.Map<ProductVariants>(productVariantDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();
            var result = await _productVariantServices.AddAsync(dataAdd);
            if (result > 0)
            {
                return Ok("Add success");
            }
            return StatusCode(500, "Add failed");
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteVariant(Guid id)
        {
            var result = await _productVariantServices.DeleteAsync(id);
            if (result)
            {
                return Ok("Delete success");
            }
            return NotFound("Delete failed");
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateVariant(Guid id, ProductVariantDto productVariantDto)
        {
            var dataUpdate = await _productVariantServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                return NotFound("Variant not found");
            }
            _mapper.Map(productVariantDto, dataUpdate);
            var result = await _productVariantServices.UpdateAsync(dataUpdate);
            if (result)
            {
                return Ok("Update success");
            }
            return StatusCode(500, "Update failed");
        }
    }
}
