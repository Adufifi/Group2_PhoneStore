using System.ComponentModel.DataAnnotations;

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
