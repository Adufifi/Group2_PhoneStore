

namespace PhoneStore.Application.Dto
{
    public class ProductVariantsDto
    {
        public required Guid ProductId { get; set; }
        public required Guid ColorId { get; set; }
        public required Guid ProductImageId { get; set; }
        public required Guid CapacityId { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity
        {
            get; set;
        }
        [Range(0, double.MaxValue)]
        public required double Price { get; set; }
    }
}