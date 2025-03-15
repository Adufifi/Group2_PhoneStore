namespace PhoneStore.Domain.Models
{

    [Table("Cart")]
    public class Cart : Base
    {
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}