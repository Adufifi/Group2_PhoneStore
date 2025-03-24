namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrderController(IOrderServices orderServices, IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }
        [HttpGet("getAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderServices.GetAllAsync();
            return Ok(orders);
        }
        [HttpGet("getOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderServices.GetByIdAsync(id);
            return Ok(order);
        }
        [HttpPost("createOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid model";
                statusResponse.status = -999;
                return BadRequest(statusResponse);
            }
            var order = _mapper.Map<Order>(orderDto);
            var check = await _orderServices.AddAsync(order);
            if (check > 0)
            {
                statusResponse.mess = "Create order success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Create order fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }
        [HttpPut("updateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDto orderDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid model";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var dataUpdate = await _orderServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                statusResponse.mess = "Order not found";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
            var order = _mapper.Map<Order>(orderDto);
            order.Id = id;
            var check = await _orderServices.UpdateAsync(order);
            if (check)
            {
                statusResponse.mess = "Update order success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Update order fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }
        [HttpDelete("deleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var statusResponse = new StatusResponse();
            var dataDelete = await _orderServices.GetByIdAsync(id);
            if (dataDelete == null)
            {
                statusResponse.mess = "Order not found";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
            var check = await _orderServices.DeleteAsync(id);
            if (check)
            {
                statusResponse.mess = "Delete order success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Delete order fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }

    }
}