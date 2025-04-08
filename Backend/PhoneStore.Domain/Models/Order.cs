using PhoneStore.Domain.Enums;

namespace PhoneStore.Domain.Models
{
    public class Order : Base
    {
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusProduct StatusProduct { get; set; }
        [StringLength(1000)]
        public string ShippingAddress { get; set; } = string.Empty;
        [StringLength(1000)]
        public string RecipientName { get; set; } = string.Empty;
        [StringLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();
    }
}