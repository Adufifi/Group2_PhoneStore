namespace PhoneStore.Domain.Models
{
    [Table("Product")]
    public class Product : Base
    {
        public Guid BrandId { get; set; }
        public virtual Brand? Brand { get; set; }
        [StringLength(100)]
        public required string ProductName { get; set; }
        public byte[]? Image { get; set; }
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        public bool IsPromoted { get; set; }
        public int? BuyCount { get; set; }
        public ICollection<Review> ProductReview { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}