using System.Threading.Tasks;
using Azure;

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

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartServices.GetAllAsync();
            return Ok(cart);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(Guid id)
        {
            var statusResponse = new StatusResponse();
            try
            {
                var result = await _cartServices.DeleteAsync(id);
                if (result)
                {
                    statusResponse.status = 1;
                    statusResponse.mess = "Delete success";
                    return Ok(statusResponse);
                }
                statusResponse.status = -99;
                statusResponse.mess = "Delete fail";
                return Ok(statusResponse);
            }
            catch (RequestFailedException ex)
            {
                statusResponse.status = -9999;
                statusResponse.mess = ex.Message;
                return BadRequest(statusResponse);
            }
        }

    }
}
