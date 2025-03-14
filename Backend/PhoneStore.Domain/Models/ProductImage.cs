namespace PhoneStore.Domain.Models
{
    [Table("ProductImage")]
    public class ProductImage : Base
    {
        public Guid ProductVariantId { get; set; }
        public virtual ProductVariants? ProductVariants { get; set; }
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}