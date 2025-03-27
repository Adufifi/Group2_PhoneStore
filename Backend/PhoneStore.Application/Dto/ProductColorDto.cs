using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Application.Dto
{
    public class ProductColorDto
    {
        [StringLength(100)]
        public required string ColorName { get; set; }
    }
}