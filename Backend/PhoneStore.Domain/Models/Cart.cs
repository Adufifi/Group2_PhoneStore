namespace PhoneStore.Domain.Models
{

    [Table("Cart")]
    public class Cart : Base
    {
        public Guid ProductId { get; set; }
        public ICollection<Product> Product { get; set; } = new List<Product>();
        public Guid AccountId { get; set; }
        public ICollection<Account> Account { get; set; } = new List<Account>();
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}