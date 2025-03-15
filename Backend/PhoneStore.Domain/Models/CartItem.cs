namespace PhoneStore.Domain.Models
{
    [Table("CardItem")]
    public class CartItem : Base
    {
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}