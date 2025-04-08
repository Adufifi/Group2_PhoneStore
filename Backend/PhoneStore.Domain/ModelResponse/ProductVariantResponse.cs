namespace PhoneStore.Domain.ModelResponse
{
    public class ProductVariantResponse
    {
        public string ProductName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string CapacityName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public byte[]? Image { get; set; }
        public double Price { get; set; }
    }
}