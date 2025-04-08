using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneStore.Domain.Enums;

namespace PhoneStore.Application.Dto
{
    public class CreateOrderDto
    {
        public Guid AccountId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }
}