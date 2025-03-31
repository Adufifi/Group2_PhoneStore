namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageServices _productImageServices;
        private readonly IMapper _mapper;

        public ProductImageController(IProductImageServices productImageServices, IMapper mapper)
        {
            _productImageServices = productImageServices;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productImage = await _productImageServices.GetAllAsync();
            return Ok(productImage);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var productImage = await _productImageServices.GetByIdAsync(id);
            return Ok(productImage);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductImageDto productImageDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid Model State";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var productImage = _mapper.Map<ProductImage>(productImageDto);
            productImage.Id = Guid.NewGuid();
            productImage.CreatedDate = DateTime.Now;
            var result = await _productImageServices.AddAsync(productImage);
            if (result > 0)
            {
                statusResponse.mess = "Add Success";
                statusResponse.status = 1;
            }
            else
            {
                statusResponse.mess = "Add Fail";
                statusResponse.status = 0;
            }
            return Ok(productImage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(Guid id, [FromBody] ProductImageDto productImageDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid Model State";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var productImage = _mapper.Map<ProductImage>(productImageDto);
            productImage.Id = id;
            productImage.CreatedDate = DateTime.Now;
            var result = await _productImageServices.UpdateAsync(productImage);
            if (result)
            {
                statusResponse.mess = "Update Success";
                statusResponse.status = 1;
            }
            else
            {
                statusResponse.mess = "Update Fail";
                statusResponse.status = 0;
            }
            return Ok(statusResponse);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(Guid id)
        {
            var statusResponse = new StatusResponse();
            var result = await _productImageServices.DeleteAsync(id);
            if (result)
            {
                statusResponse.mess = "Delete Success";
                statusResponse.status = 1;
            }
            else
            {
                statusResponse.mess = "Delete Fail";
                statusResponse.status = 0;
            }
            return Ok(statusResponse);
        }
    }
}