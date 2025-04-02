namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductColorController : ControllerBase
    {
        private readonly IProductColorServices _productColorServices;
        private readonly IMapper _mapper;

        public ProductColorController(IProductColorServices productColorServices, IMapper mapper)
        {
            _productColorServices = productColorServices;
            _mapper = mapper;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var data = await _productColorServices.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("GetColorById/{id}")]
        public async Task<IActionResult> GetColorById(Guid id)
        {
            var data = await _productColorServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("CreateColor")]
        public async Task<IActionResult> CreateColor([FromBody] ProductColorDto colorDto)
        {
            var dataAdd = _mapper.Map<ProductColor>(colorDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();
            var result = await _productColorServices.AddAsync(dataAdd);
            if (result > 0)
            {
                return Ok("Add success");
            }
            return StatusCode(500, "Add failed");
        }

        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteColor(Guid id)
        {
            var result = await _productColorServices.DeleteAsync(id);
            if (result)
            {
                return Ok("Delete success");
            }
            return NotFound("Delete failed");
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateColor(Guid id, ProductColorDto colorDto)
        {
            var dataUpdate = await _productColorServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                return NotFound("Color not found");
            }
            _mapper.Map(colorDto, dataUpdate);
            var result = await _productColorServices.UpdateAsync(dataUpdate);
            if (result)
            {
                return Ok("Update success");
            }
            return StatusCode(500, "Update failed");
        }
    }
}