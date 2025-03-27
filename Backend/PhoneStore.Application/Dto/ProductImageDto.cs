using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Application.Dto
{
    public class ProductImageDto
    {
        public Guid ProductVariantId { get; set; }
        public byte[] Img { get; set; } = default!;
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}