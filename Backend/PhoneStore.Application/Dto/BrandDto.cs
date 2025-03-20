using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Application.Dto
{
    public class BrandDto
    {
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
