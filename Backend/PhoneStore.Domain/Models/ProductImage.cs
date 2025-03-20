namespace PhoneStore.Domain.Models
{
    [Table("ProductImage")]
    public class ProductImage : Base
    {
        public Guid ProductVariantId { get; set; }
        [ForeignKey("ProductVariantId")]
        public virtual ProductVariants? ProductVariants { get; set; }
        public byte[] Img { get; set; } = default!;
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
    }
}