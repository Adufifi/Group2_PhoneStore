

namespace PhoneStore.Application.Dto
{
    public class ProductDto
    {
        public Guid BrandId { get; set; }
        [StringLength(100)]
        public required string ProductName { get; set; }
        public byte[]? Image { get; set; }
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public bool IsPromoted { get; set; }
        [Range(0, int.MaxValue)]
        public int? BuyCount { get; set; }
    }
}