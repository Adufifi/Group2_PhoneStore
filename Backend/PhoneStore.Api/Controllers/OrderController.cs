using PhoneStore.Domain.Enums;

namespace PhoneStore.Api.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly IProductVariantsServices _productVariantsServices;
        private readonly ICartServices _cartServices;
        private readonly IProductServices _productServices;

        public OrderController(IOrderServices orderServices, IMapper mapper, IProductVariantsServices productVariantsServices, ICartServices cartServices, IProductServices productServices)
        {
            _orderServices = orderServices;
            _mapper = mapper;
            _productVariantsServices = productVariantsServices;
            _cartServices = cartServices;
            _productServices = productServices;
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
                return Ok(statusResponse);
            }

            try
            {
                var listCart = await _cartServices.GetAllCart(orderDto.AccountId);

                if (listCart == null || !listCart.Any())
                {
                    statusResponse.mess = "Cart is empty";
                    statusResponse.status = -1;
                    return Ok(statusResponse);
                }
                var orderAdd = new Order()
                {
                    AccountId = orderDto.AccountId,
                    PaymentMethod = orderDto.PaymentMethod,
                    StatusProduct = StatusProduct.Pending,
                    ShippingAddress = orderDto.ShippingAddress,
                    RecipientName = orderDto.RecipientName,
                    PhoneNumber = orderDto.PhoneNumber,
                    CreatedDate = DateTime.Now
                };
                foreach (var cartItem in listCart)
                {
                    var productVariant = await _productVariantsServices.GetByIdAsync(cartItem.ProductVariantsId);

                    if (productVariant == null)
                    {
                        statusResponse.mess = $"ProductVariant with ID {cartItem.ProductVariantsId} not found.";
                        statusResponse.status = -2;
                        return Ok(statusResponse);
                    }

                    if (productVariant.Quantity < cartItem.Quantity)
                    {
                        statusResponse.mess = $"ProductVariant with ID {cartItem.ProductVariantsId} does not have enough stock.";
                        statusResponse.status = -3;
                        return Ok(statusResponse);
                    }
                    productVariant.Quantity -= cartItem.Quantity;
                    orderAdd.ProductVariants.Add(productVariant);
                    var product = await _productServices.GetByIdAsync(productVariant.ProductId);
                    if (product == null)
                    {
                        statusResponse.mess = $"Product with ID {productVariant.ProductId} not found.";
                        statusResponse.status = -2;
                        return Ok(statusResponse);
                    }
                    product.BuyCount++;
                    await _productServices.UpdateAsync(product);
                }
                var result = await _orderServices.AddAsync(orderAdd);

                if (result > 0)
                {
                    await _cartServices.DeleteAllAsync(orderDto.AccountId);

                    statusResponse.mess = "Order created successfully";
                    statusResponse.status = 1;
                    return Ok(statusResponse);
                }

                statusResponse.mess = "Failed to create order";
                statusResponse.status = -4;
                return Ok(statusResponse);
            }
            catch (Exception ex)
            {
                statusResponse.mess = $"Error: {ex.Message}";
                statusResponse.status = -500;
                return StatusCode(500, statusResponse);
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
        [HttpGet("getAllOrderByAccountId/{accountId}")]
        public async Task<IActionResult> GetAllOrderByAccountId(string accountId)
        {
            if (Guid.TryParse(accountId, out Guid accountIdGuid) == false)
            {
                return BadRequest("Invalid account ID format.");
            }
            var orders = await _orderServices.GetAllOrderByAccountId(accountIdGuid);
            var orderResponse = orders.Select(o => new OrderResponse
            {
                CreatedDate = o.CreatedDate,
                PaymentMethod = o.PaymentMethod,
                StatusProduct = o.StatusProduct,
                ShippingAddress = o.ShippingAddress,
                RecipientName = o.RecipientName,
                PhoneNumber = o.PhoneNumber,
                ProductVariantResponse = o.ProductVariants.Select(pv => new ProductVariantResponse
                {
                    ProductName = pv.Product.ProductName,
                    ColorName = pv.ProductColor.ColorName,
                    CapacityName = pv.Capacity.CapacityName,
                    Price = pv.Price,
                    Quantity = pv.Quantity,
                    Image = pv.Image

                }).ToList()
            }).ToList();
            return Ok(orderResponse);
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllOrder()
        {
            var orders = await _orderServices.GetAllOrder();
            var orderResponse = orders.Select(o => new OrderResponse
            {
                CreatedDate = o.CreatedDate,
                PaymentMethod = o.PaymentMethod,
                StatusProduct = o.StatusProduct,
                ShippingAddress = o.ShippingAddress,
                RecipientName = o.RecipientName,
                PhoneNumber = o.PhoneNumber,
                ProductVariantResponse = o.ProductVariants.Select(pv => new ProductVariantResponse
                {
                    ProductName = pv.Product.ProductName,
                    ColorName = pv.ProductColor.ColorName,
                    CapacityName = pv.Capacity.CapacityName,
                    Price = pv.Price,
                    Quantity = pv.Quantity,
                    Image = pv.Image

                }).ToList()
            }).ToList();
            return Ok(orderResponse);
        }

    }
}