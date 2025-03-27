using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Application.Dto
{
    public class CapacityDto
    {
        [StringLength(100)]
        public required string CapacityName { get; set; }
    }
}