namespace PhoneStore.Api.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly IMapper _mapper;
        private readonly IOrderServices _orderServices;
        private readonly IReviewServices _reviewServices;
        private readonly ICartServices _cartServices;
        private readonly IOrderItemServices _orderItemServices;
        private readonly IProductVariantsServices _productVariantsServices;
        private readonly IProductImageServices _productImageServices;

        public ProductController(IProductServices productServices, IMapper mapper,
        IOrderServices orderServices, IReviewServices reviewServices, ICartServices cartServices, IOrderItemServices orderItemServices,
        IProductVariantsServices productVariantsServices, IProductImageServices productImageServices)
        {
            _productServices = productServices;
            _mapper = mapper;
            _orderServices = orderServices;
            _reviewServices = reviewServices;
            _cartServices = cartServices;
            _orderItemServices = orderItemServices;
            _productVariantsServices = productVariantsServices;
            _productImageServices = productImageServices;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var data = await _productServices.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var data = await _productServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return NotFound(statusResponse);
            }
            var dataAdd = _mapper.Map<Product>(productDto);
            dataAdd.CreatedDate = DateTime.Now;
            dataAdd.Id = Guid.NewGuid();
            var result = await _productServices.AddAsync(dataAdd);
            if (result > 0)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Add success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Add valid";
            return StatusCode(500, statusResponse);
        }

        // [HttpDelete("DeleteById/{id}")]
        // public async Task<IActionResult> DeleteProduct(Guid id)
        // {

        // }

        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, ProductDto productDto)
        {
            var statusResponse = new StatusResponse();
            var dataUpdate = await _productServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Product not found";
                return NotFound(statusResponse);
            }
            if (!ModelState.IsValid)
            {
                statusResponse.status = -909;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }
            _mapper.Map(productDto, dataUpdate);
            var result = await _productServices.UpdateAsync(dataUpdate);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Update success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Update valid";
            return StatusCode(500, statusResponse);
        }
    }
}
