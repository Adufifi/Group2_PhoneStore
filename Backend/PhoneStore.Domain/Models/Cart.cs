namespace PhoneStore.Domain.Models
{

    [Table("Cart")]
    public class Cart : Base
    {
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
    }
}