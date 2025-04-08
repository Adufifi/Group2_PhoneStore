using PhoneStore.Domain.Enums;

namespace PhoneStore.Domain.ModelResponse
{
    public class OrderResponse
    {
        public DateTime CreatedDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusProduct StatusProduct { get; set; }
        [StringLength(1000)]
        public string ShippingAddress { get; set; } = string.Empty;
        [StringLength(1000)]
        public string RecipientName { get; set; } = string.Empty;
        [StringLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;

        public List<ProductVariantResponse> ProductVariantResponse { get; set; } = new List<ProductVariantResponse>();

    }

}
