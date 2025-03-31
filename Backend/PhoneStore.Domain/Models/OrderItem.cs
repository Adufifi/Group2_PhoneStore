namespace PhoneStore.Domain.Models
{
    public class OrderItem : Base
    {
        public Guid ProductVariantsId { get; set; }
        public virtual ProductVariants? Product { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order? Order { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
    }
}