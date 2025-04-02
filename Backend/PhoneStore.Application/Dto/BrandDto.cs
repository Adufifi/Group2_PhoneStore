using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class BrandDto
    {
        [StringLength(50)]
        public required string Name { get; set; }
    }
}
