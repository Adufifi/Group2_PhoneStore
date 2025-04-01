namespace PhoneStore.Domain.Models
{
    [Table("ProductVariant")]
    public class ProductVariants : Base
    {
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public required Guid ColorId { get; set; }
        public virtual ProductColor? ProductColor { get; set; }
        public Guid? ProductImageId { get; set; }
        [ForeignKey("ProductImageId")]
        public virtual ProductImage? ProductImages { get; set; }
        public required Guid CapacityId { get; set; }
        public virtual Capacity? Capacity { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity
        {
            get; set;
        }
        [Range(0, double.MaxValue)]
        public required double Price { get; set; }
        public Guid CartId { get; set; }
        public required Cart Carts { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    }
}