namespace PhoneStore.Domain.Models
{
    [Table("Color")]
    public class ProductColor : Base
    {
        [StringLength(255)]
        public required string ColorName { get; set; }

    }
}