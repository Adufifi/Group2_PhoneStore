namespace PhoneStore.Application.Dto
{
    public class ProductVariantsDto
    {
        public required Guid ProductId { get; set; }
        public string? ProductName { get; set; }

        public required Guid ColorId { get; set; }
        public string? ColorName { get; set; }
        public byte[]? Image { get; set; }
        public required Guid CapacityId { get; set; }
        public string? CapacityName { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity
        {
            get; set;
        }
        [Range(0, double.MaxValue)]
        public required double Price { get; set; }
    }
}