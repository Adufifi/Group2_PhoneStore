namespace PhoneStore.Domain.Models
{
    [Table("ProductVariant")]
    public class ProductVariants : Base
    {
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public required Guid ColorId { get; set; }
        public virtual ProductColor? ProductColor { get; set; }
<<<<<<< HEAD:PhoneStore.Domain/Models/ProductVariants.cs
=======
        public required Guid ProductImageId { get; set; }
        [ForeignKey("ProductImageId")]
        public virtual ProductImage? ProductImages { get; set; }
        public required Guid CapacityId { get; set; }
        public virtual Capacity? Capacity { get; set; }

>>>>>>> 41a6b7ead206b44440065dbb6e01b8e681c23bbf:Backend/PhoneStore.Domain/Models/ProductVariants.cs
        [Range(0, int.MaxValue)]
        public int Quantity
        {
            get; set;
        }
<<<<<<< HEAD:PhoneStore.Domain/Models/ProductVariants.cs
        public Guid CapacityId { get; set; }
        public virtual Capacity? Capacity { get; set; }
=======
>>>>>>> 41a6b7ead206b44440065dbb6e01b8e681c23bbf:Backend/PhoneStore.Domain/Models/ProductVariants.cs
        [Range(0, double.MaxValue)]
        public required double Price { get; set; }

    }
}