using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class ProductVariantDto
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string VariantName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
