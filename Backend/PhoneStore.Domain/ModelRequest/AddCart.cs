namespace PhoneStore.Domain.ModelRequest
{
    public class AddCart
    {
        public required string AccountId { get; set; }
        public required string ProductVariantId { get; set; }
    }
}
