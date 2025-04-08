namespace PhoneStore.Application.Dto
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public string CapacityName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public byte[]? Image { get; set; }
        public string? BrandName { get; set; } = string.Empty;
    }
}
