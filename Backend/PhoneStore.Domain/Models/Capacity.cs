using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Domain.Models
{
    public class Capacity : Base
    {
        public required string CapacityName { get; set; }
        public virtual ProductVariants? ProductVariant { get; set; }
    }
}