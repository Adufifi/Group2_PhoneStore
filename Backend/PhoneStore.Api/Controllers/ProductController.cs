using PhoneStore.Api.Helper;

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
        private readonly IProductVariantsServices _productVariantsServices;

        public ProductController(IProductServices productServices, IMapper mapper,
        IOrderServices orderServices, IReviewServices reviewServices, ICartServices cartServices,
        IProductVariantsServices productVariantsServices)
        {
            _productServices = productServices;
            _mapper = mapper;
            _orderServices = orderServices;
            _reviewServices = reviewServices;
            _cartServices = cartServices;
            _productVariantsServices = productVariantsServices;
        }

        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var products = await _productServices.GetAlllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
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
                return BadRequest(statusResponse);
            }

            try
            {
                var product = _mapper.Map<Product>(productDto);
                product.CreatedDate = DateTime.Now;
                product.Id = Guid.NewGuid();

                if (!string.IsNullOrEmpty(productDto.ImagePath))
                {
                    product.Image = ImageHelper.ConvertImageToByteArray(productDto.ImagePath);
                }

                var result = await _productServices.AddAsync(product);

                if (result > 0)
                {
                    statusResponse.status = 1;
                    statusResponse.mess = "Add success";
                    return Ok(statusResponse);
                }

                statusResponse.status = -2;
                statusResponse.mess = "Failed to add product";
                return StatusCode(500, statusResponse);
            }
            catch (Exception ex)
            {
                statusResponse.status = -3;
                statusResponse.mess = $"Error creating product: {ex.Message}";
                return StatusCode(500, statusResponse);
            }
        }
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
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            var statusResponse = new StatusResponse();
            if (Guid.TryParse(id, out Guid productId) == false)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Id not found";
                return NotFound(statusResponse);
            }
            var result = await _productServices.DeleteAsync(productId);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }
            statusResponse.status = -2;
            statusResponse.mess = "Delete valid";
            return StatusCode(500, statusResponse);
        }
    }
}
