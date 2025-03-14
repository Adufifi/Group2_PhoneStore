namespace PhoneStore.Domain.Models
{
    [Table("Brand")]
    public class Brand : Base
    {
        [StringLength(50)]
        public required string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}