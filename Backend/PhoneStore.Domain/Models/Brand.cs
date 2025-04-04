namespace PhoneStore.Domain.Models
{
    [Table("Brand")]
    public class Brand : Base
    {
        [StringLength(50)]
        public required string Name { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}