namespace PhoneStore.Domain.Models
{
    [Table("ProductVariant")]
    public class ProductVariants : Base
    {
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public required Guid ColorId { get; set; }
        public virtual ProductColor? ProductColor { get; set; }
        public Guid CapacityId { get; set; }
        public virtual Capacity? Capacity { get; set; }
        [Range(0, double.MaxValue)]
        public required double Price { get; set; }

    }
}