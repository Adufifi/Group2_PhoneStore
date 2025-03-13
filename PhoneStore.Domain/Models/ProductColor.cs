namespace PhoneStore.Domain.Models
{
    [Table("Color")]
    public class ProductColor : Base
    {
        public required string ColorName { get; set; }
    }
}