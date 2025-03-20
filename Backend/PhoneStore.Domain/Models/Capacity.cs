namespace PhoneStore.Domain.Models
{
    public class Capacity : Base
    {
        [StringLength(100)]
        public required string CapacityName { get; set; }
        public virtual ProductVariants? ProductVariant { get; set; }
    }
}