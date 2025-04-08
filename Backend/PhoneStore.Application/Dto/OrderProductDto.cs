using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Application.Dto
{
    public class OrderProductDto
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}