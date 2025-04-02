namespace PhoneStore.Domain.Models
{
    public class Capacity : Base
    {
        [StringLength(100)]
        public required string CapacityName { get; set; }
        public ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();
    }
}