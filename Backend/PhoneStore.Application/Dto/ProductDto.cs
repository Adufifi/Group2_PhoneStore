

namespace PhoneStore.Application.Dto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string? BrandName { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string? ImagePath { get; set; }
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public bool IsPromoted { get; set; }
        [Range(0, int.MaxValue)]
        public int? BuyCount { get; set; }
    }
}