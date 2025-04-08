namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductVariantsServices _productVariantsServices;
        private readonly IMapper _mapper;

        public ProductVariantsController(IProductVariantsServices productVariantsServices, IMapper mapper)
        {
            _productVariantsServices = productVariantsServices;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productVariants = await _productVariantsServices.GetAllAsync();
            return Ok(productVariants);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var productVariants = await _productVariantsServices.GetByIdAsync(id);
            return Ok(productVariants);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductVariantsDto productVariantsDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid Model State";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var productVariants = _mapper.Map<ProductVariants>(productVariantsDto);
            productVariants.Id = Guid.NewGuid();
            productVariants.CreatedDate = DateTime.Now;
            var result = await _productVariantsServices.AddAsync(productVariants);
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
            return Ok(productVariants);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductVariants(Guid id, [FromBody] ProductVariantsDto productVariantsDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid Model State";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var productVariants = _mapper.Map<ProductVariants>(productVariantsDto);
            productVariants.Id = id;
            var result = await _productVariantsServices.UpdateAsync(productVariants);
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
        public async Task<IActionResult> DeleteProductVariants(Guid id)
        {
            var statusResponse = new StatusResponse();
            var result = await _productVariantsServices.DeleteAsync(id);
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
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            var allVariants = await _productVariantsServices.GetAllAsync();
            var productVariants = allVariants.Where(v => v.ProductId == productId).ToList();
            var productVariantsDtos = _mapper.Map<IEnumerable<ProductVariantsDto>>(productVariants);
            return Ok(productVariantsDtos);
        }
    }
}