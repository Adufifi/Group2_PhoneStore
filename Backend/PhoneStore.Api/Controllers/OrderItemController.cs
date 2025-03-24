namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemServices _orderItemServices;
        private readonly IMapper _mapper;

        public OrderItemController(IOrderItemServices orderItemServices, IMapper mapper)
        {
            _orderItemServices = orderItemServices;
            _mapper = mapper;
        }
        [HttpGet("getAllOrderItems")]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var orderItems = await _orderItemServices.GetAllAsync();
            return Ok(orderItems);
        }
        [HttpGet("getOrderItemById/{id}")]
        public async Task<IActionResult> GetOrderItemById(Guid id)
        {
            var orderItem = await _orderItemServices.GetByIdAsync(id);
            return Ok(orderItem);
        }
        [HttpPost("createOrderItem")]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemDto orderItemDto)
        {
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid model";
                statusResponse.status = -999;
                return BadRequest(statusResponse);
            }
            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            var check = await _orderItemServices.AddAsync(orderItem);
            if (check > 0)
            {
                statusResponse.mess = "Create orderItem success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Create orderItem fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }
        [HttpPut("updateOrderItem/{id}")]
        public async Task<IActionResult> UpdateOrderItem(Guid id, [FromBody] OrderItemDto orderItemDto)
        {
            var statusResponse = new StatusResponse();
            var dataUpdate = await _orderItemServices.GetByIdAsync(id);
            if (dataUpdate == null)
            {
                statusResponse.mess = "OrderItem not found";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
            if (!ModelState.IsValid)
            {
                statusResponse.mess = "Invalid model";
                statusResponse.status = -999;
                return Ok(statusResponse);
            }
            var orderItem = _mapper.Map<OrderItem>(orderItemDto);
            orderItem.Id = id;
            var check = await _orderItemServices.UpdateAsync(orderItem);
            if (check)
            {
                statusResponse.mess = "Update orderItem success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Update orderItem fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }
        [HttpDelete("deleteOrderItem/{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id)
        {
            var statusResponse = new StatusResponse();
            var orderItem = await _orderItemServices.GetByIdAsync(id);
            if (orderItem == null)
            {
                statusResponse.mess = "OrderItem not found";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
            var check = await _orderItemServices.DeleteAsync(orderItem);
            if (check)
            {
                statusResponse.mess = "Delete orderItem success";
                statusResponse.status = 1;
                return Ok(statusResponse);
            }
            else
            {
                statusResponse.mess = "Delete orderItem fail";
                statusResponse.status = 0;
                return Ok(statusResponse);
            }
        }

    }
}