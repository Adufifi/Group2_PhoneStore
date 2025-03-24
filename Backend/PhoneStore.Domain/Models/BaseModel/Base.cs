namespace PhoneStore.Domain.Models
{
    public abstract class Base
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}